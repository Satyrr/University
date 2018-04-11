#include "route.h"

#define INFINITY_DIST 0xffffffff

// checks if any of interfaces has given ip
int own_ip (
	unsigned char ip[4], 
	Interface* interfaces,
	int interface_number
	){

	int correct_no = 0;
	for(int i = 0; i < interface_number; i++)
	{
		for(int k = 0; k < 4; k++)
			if(interfaces[i].ip_bytes[k] == ip[k])
				correct_no++;

		if(correct_no == 4)
			return i;
		else
			correct_no = 0;
	}

	return NOT_FOUND;
}

// checks if any of interfaces belongs to given network
int own_network (
	unsigned char ip[4], 
	Interface* interfaces,
	int interface_number
	){

	int correct_no = 0;
	for(int i = 0; i < interface_number; i++)
	{
		unsigned char network_ip[4];
		network_addr(
			interfaces[i].ip_bytes, 
			interfaces[i].mask, 
			network_ip
			);

		for(int k = 0; k < 4; k++)
			if(network_ip[k] == ip[k])
				correct_no++;

		if(correct_no == 4)
			return i;
		else
			correct_no = 0;
	}

	return NOT_FOUND;
}

// returns distance to neighbour with neigh_ip
unsigned int neighbour_distance(
	Interface* in,
 	int in_number, 
	unsigned char* neigh_ip){

	int correct_no = 0;

	for(int i = 0; i < in_number; i++)
	{
		unsigned char network_neigh_ip[4];
		network_addr(
			neigh_ip, 
			in[i].mask, 
			network_neigh_ip
			);

		unsigned char network_interface_ip[4];
		network_addr(
			in[i].ip_bytes, 
			in[i].mask, 
			network_interface_ip
			);

		for(int k = 0; k < 4; k++)
			if(network_interface_ip[k] == network_neigh_ip[k])
				correct_no++;

		if(correct_no == 4)
			return in[i].distance;
		else
			correct_no = 0;
	}
	return 0;
}

void neighbour_messages(
	int sockfd,
	Interface* interfaces,
	int interface_number
	){
	for(int i = 0; i < interface_number; i++)
	{
		unsigned char bc_ip[4];
		broadcast_addr(
			interfaces[i].ip_bytes,
			interfaces[i].mask,
			bc_ip 
			);

		struct sockaddr_in server_address = new_sockaddr_in(bc_ip, 54321);

		unsigned char network_ip[4];
		network_addr(
			interfaces[i].ip_bytes,
			interfaces[i].mask,
			network_ip 
			);

		unsigned char message[9];
		int distance = htonl(interfaces[i].distance);

		memcpy(message, network_ip, 4);
		message[4] = interfaces[i].mask;
		memcpy(&message[5], &distance, 4);

		if (sendto(
					sockfd, 
					message, 
					9, 
					0, 
					(struct sockaddr*) &server_address, 
					sizeof(server_address)
					) != 9){
			fprintf(stderr, "neighbour_messages error: %s\n", strerror(errno));
		}
	}
}

void interface_unreachable_update(
	Interface* in,
	int index,
	Vector *v)
{
	// set unreachable interface element and all its via 
	// elements distance to infinity
	for(int i = 0; i < v->number; i++)
	{

		unsigned char n_addr[4];
		network_addr(
			in[index].ip_bytes,
			in[index].mask,
			n_addr);

		if( v->elements[i].ip_bytes[0] == n_addr[0] &&
			v->elements[i].ip_bytes[1] == n_addr[1] &&
			v->elements[i].ip_bytes[2] == n_addr[2] &&
			v->elements[i].ip_bytes[3] == n_addr[3] &&
			v->elements[i].mask == in[index].mask
		){
				v->elements[i].distance = INFINITY_DIST;
		}
		else if( 
			v->elements[i].is_neighbour == 0 &&
			v->elements[i].via_ip[0] == in[index].ip_bytes[0] &&
			v->elements[i].via_ip[1] == in[index].ip_bytes[1] &&
			v->elements[i].via_ip[2] == in[index].ip_bytes[2] &&
			v->elements[i].via_ip[3] == in[index].ip_bytes[3]
			){

				v->elements[i].distance = INFINITY_DIST;
		}
	}
}
