#include "route.h"

// prints current vector
void show_vector(Vector *v)
{
	for(int i = 0; i < v->number; i++)
	{
		if(v->elements[i].is_neighbour == 0 &&
			v->elements[i].no_respond_turns > 3)
			continue;

		printf("\n%d.%d.%d.%d/%d ",
				v->elements[i].ip_bytes[0],
				v->elements[i].ip_bytes[1],
				v->elements[i].ip_bytes[2],
				v->elements[i].ip_bytes[3],
				v->elements[i].mask
				);

		if(v->elements[i].distance < INFINITY_DIST)
			printf("distance %d ", v->elements[i].distance);
		else
			printf("unreachable ");

		if(v->elements[i].is_neighbour == 1)
			printf("connected directly");
		else if(v->elements[i].distance < INFINITY_DIST)
			printf("via %d.%d.%d.%d",
				v->elements[i].via_ip[0],
				v->elements[i].via_ip[1],
				v->elements[i].via_ip[2],
				v->elements[i].via_ip[3]);
	}
	printf("\n\n\n");
}

// fills vector with neighbour networks
void init_vector (
	Vector *v,
	Interface *interfaces,
	int interface_number
	){

	for(int i = 0; i < interface_number; i++)
	{
		unsigned char n_addr[4];
		network_addr(
			interfaces[i].ip_bytes,
			interfaces[i].mask,
			n_addr
			);

		new_element(
			v,
			n_addr,
			interfaces[i].mask,
			INFINITY_DIST,
			NEIGHBOUR,
			NULL);
	}
}

// adds element to vector
void new_element(Vector *v,
		unsigned char ip[4],
		unsigned char mask,
		unsigned int distance,
		int is_neighbour,
		unsigned char via_ip[4]){

	int index = v->number;
	v->number++;

	for(int i = 0; i < 4; i++)
		v->elements[index].ip_bytes[i] = ip[i];

	v->elements[index].mask = mask;
	v->elements[index].distance = distance;
	v->elements[index].is_neighbour = is_neighbour;
	v->elements[index].no_respond_turns = 0;

	if(is_neighbour == 0) 
	{
		for(int i = 0; i < 4; i++)
			v->elements[index].via_ip[i] = via_ip[i];
	}
}

// search for element in vector, return its index
int find_element_index(Vector *v, 
			unsigned char ip[4], 
			unsigned char mask){

	for(int i = 0; i < v->number; i++)
	{
		int found = 1;
		for(int k = 0; k < 4; k++)
			if(v->elements[i].ip_bytes[k] != ip[k])
				found = 0;
		if(v->elements[i].mask != mask) found = 0;

		if(found == 1) return i;
	}

	return NOT_FOUND;
}

// increments no respond counter in vector elements
void update_no_respond(Vector *v)
{
	for(int i = 0; i < v->number; i++)
	{
		v->elements[i].no_respond_turns++;
		if(v->elements[i].no_respond_turns > 3)
			v->elements[i].distance = INFINITY_DIST;
	}
}

// sends vector to all neighbour networks
int send_vector(int sockfd,
	Interface* in,
	int in_number,
	Vector *v){

	// iterate interfaces
	for(int i = 0; i < in_number; i++)
	{
		// interface broadcast ip
		unsigned char bc_ip[4];
		broadcast_addr(in[i].ip_bytes, in[i].mask, bc_ip );

		struct sockaddr_in server_address = new_sockaddr_in(bc_ip, 54321);

		int interface_unreachable = 0;

		// iterate vector
		for(int k = 0; k < v->number; k++)
		{
			if(v->elements[k].no_respond_turns > 3) continue;

			unsigned char message[9];
			int distance = htonl(v->elements[k].distance);

			memcpy(message, v->elements[k].ip_bytes, 4);
			message[4] = v->elements[k].mask;
			memcpy(&message[5], &distance, 4);

			if (sendto(
					sockfd, 
					message, 
					9, 
					0, 
					(struct sockaddr*) &server_address, 
					sizeof(server_address)
					) != 9) {

				printf("Interface unreachable\n");
				interface_unreachable = 1;
				break;
			}
		}

		if(interface_unreachable == 1)
			interface_unreachable_update(in, i, v);

	}

	return SUCCESS;
}

int receive_vectors(Vector *v,
					int sockfd,
					Interface* interfaces,
					int interface_number
					){

	// increment no respond counter
	update_no_respond(v);

	fd_set descriptors;
	FD_ZERO (&descriptors);
	FD_SET (sockfd, &descriptors);
	struct timeval tv; tv.tv_sec = 7; tv.tv_usec = 0;
	
	int ready = select(sockfd+1, &descriptors, NULL, NULL, &tv);
	if(ready == -1)
	{
		fprintf(stderr, "select error: %s\n", strerror(errno));
		return EXIT_FAILURE; 
	}
	while(ready == 1)
	{
		struct sockaddr_in 	sender;	
		socklen_t 			sender_len = sizeof(sender);
		u_int8_t 			buffer[IP_MAXPACKET+1];

		ssize_t datagram_len = 
			recvfrom(sockfd, 
					buffer, 
					IP_MAXPACKET, 
					0, 
					(struct sockaddr*)&sender, 
					&sender_len);

		if (datagram_len < 0) {
			fprintf(stderr, "recvfrom error: %s\n", strerror(errno)); 
			return EXIT_FAILURE;
		}

		unsigned char ip[4];
		unsigned char mask;
		unsigned int distance;

		memcpy(ip, buffer, 4);
		mask = buffer[4];
		memcpy(&distance, &buffer[5], 4);
		distance = ntohl(distance);

		unsigned char sender_ip[4];
		int_to_bytes_ip(ntohl(sender.sin_addr.s_addr), sender_ip);

		// checks if this message was sent by this program,
		// if yes, updates only its neighbour network status in vector
		int sender_index = 
			own_ip(sender_ip,
					interfaces,
					interface_number);
		int element_interface_index = 
			own_network(ip,
						interfaces,
						interface_number);
		if(sender_index != NOT_FOUND && element_interface_index != sender_index)
		{
			ready = select (sockfd+1, &descriptors, NULL, NULL, &tv);
			if(ready == -1)
			{
				fprintf(stderr, "select error: %s\n", strerror(errno));
				return EXIT_FAILURE; 
			}
			continue;
		}

		int is_neighbour = NOT_NEIGHBOUR;

		if(element_interface_index != NOT_FOUND &&
			element_interface_index == sender_index){

			distance = 0;
			is_neighbour = NEIGHBOUR;
		} 
		unsigned int n_distance =
			neighbour_distance(interfaces, interface_number, sender_ip);

		if(distance != INFINITY_DIST)
			distance = distance + n_distance;
				

		unsigned char sender_network_ip[4];
		network_addr(sender_ip, mask, sender_network_ip);

		if(memcmp(sender_network_ip, ip, 4) == 0)
			is_neighbour = NEIGHBOUR;

		// check if received element is in vector
		int element_index = find_element_index(v, ip, mask);
		// not found - add new element
		if(element_index == NOT_FOUND)
		{
			new_element(v,
						ip,
						mask,
						distance,
						is_neighbour,
						sender_ip);
		}
		// found - update distance, via_ip and reset no_respond_turns
		else
		{
			if(distance < v->elements[element_index].distance)
			{
				v->elements[element_index].is_neighbour = is_neighbour;

				if(distance > 30)
					distance = INFINITY_DIST;

				v->elements[element_index].distance = distance;
				if(is_neighbour == NOT_NEIGHBOUR)
					for(int i = 0; i < 4; i++)
						v->elements[element_index].via_ip[i] = sender_ip[i];
				else
					for(int i = 0; i < 4; i++)
						v->elements[element_index].via_ip[i] = 0;
			}
			else if(memcmp(sender_ip, v->elements[element_index].via_ip, 4) == 0)
			{
				if(distance > 30)
					distance = INFINITY_DIST;
				v->elements[element_index].is_neighbour = is_neighbour;
				v->elements[element_index].distance = distance;
			}

			if(element_interface_index != NOT_FOUND &&
			element_interface_index == sender_index)
				v->elements[element_index].no_respond_turns = 0;
			else if(memcmp(sender_ip, v->elements[element_index].via_ip, 4) == 0)
				v->elements[element_index].no_respond_turns = 0;
		}

		// get next message
		ready = select (sockfd+1, &descriptors, NULL, NULL, &tv);
		if(ready == -1)
		{
			fprintf(stderr, "select error: %s\n", strerror(errno));
			return EXIT_FAILURE; 
		}
	}
	return SUCCESS;
}


