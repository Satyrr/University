//Marcin Gruza
//281050
#include "traceroute.h"

int pick_messages(int sockfd, struct timespec timestamps[3], char* reply, char* dest_ip, int ttl)
{
	char 				ips[2][20];
	int 				dest_reached = 0;

	struct sockaddr_in 	sender;	
	socklen_t 			sender_len = sizeof(sender);
	u_int8_t 			buffer[IP_MAXPACKET];

	fd_set descriptors;
	FD_ZERO (&descriptors);
	FD_SET (sockfd, &descriptors);
	struct timeval tv; tv.tv_sec = 1; tv.tv_usec = 0;

	int packet_num = 0;
	while (packet_num < 3)
	{
		int ready = select (sockfd+1, &descriptors, NULL, NULL, &tv);
		switch(ready)
		{
		/*
			Error
		*/
		case -1:
			fprintf(stderr, "select error: %s\n", strerror(errno)); 
			return -1;

		/*
			Timeout 
		*/
		case 0:
			if (packet_num > 0) strcat(reply, "???\n");
			else strcat(reply, "*\n");
			return dest_reached==1;

		/*
			Get messages
		*/ 
		case 1: ;
			ssize_t packet_len = 0;
			while (packet_len != -1)
			{
				packet_len = recvfrom (sockfd,
									buffer, 
									IP_MAXPACKET, 
									MSG_DONTWAIT, 
		                         	(struct sockaddr*)&sender, 
		                         	&sender_len);

				if (packet_len < 0 && errno != EWOULDBLOCK) 
				{
					fprintf(stderr, "recvfrom error: %s\n", strerror(errno)); 
					return -1;
				}

				/*
					Empty socket
				*/
				if (packet_len < 0) break;

				/*
					IP and ICMP header
				*/
				struct iphdr* ip_header = (struct iphdr*) buffer;
				u_int8_t* icmp_packet = buffer + 4 * ip_header->ihl;
				struct icmphdr* icmp_header = (struct icmphdr*) icmp_packet;
				
				/*
					Gets original ICMP packet from time exceeded ICMP reply
				*/
				if (icmp_header->type == 11)
				{
					u_int8_t* original_icmp = icmp_packet + 8;
					ip_header = (struct iphdr*) original_icmp;
					icmp_packet = original_icmp + 4 * ip_header->ihl;
					icmp_header = (struct icmphdr*) icmp_packet;
				}

				/*
					Check if received packet is a reply to us 
				*/
				if (check_message(ttl, icmp_header) == 1) 
				{
					struct timespec response_time;
					clock_gettime(CLOCK_REALTIME, &response_time); 
					// Makes timestamp
					timestamps[packet_num].tv_nsec = response_time.tv_nsec - timestamps[packet_num].tv_nsec;
					timestamps[packet_num].tv_sec = response_time.tv_sec - timestamps[packet_num].tv_sec;

					packet_num++;
					// All packets received
					if(packet_num > 2) break;
				}
				// Ignore packet
				else continue;

				/*
					Read sender's ip. Add new router's IP(if different)
				*/
				char sender_ip_str[20]; 
				inet_ntop(AF_INET, &(sender.sin_addr), sender_ip_str, sizeof(sender_ip_str));
			
				if (unique_ip(ips, packet_num, sender_ip_str) == 1)
				{
					strcat(reply, sender_ip_str);
					strcat(reply, " ");
					// Check if destination reached
					if (strcmp(sender_ip_str, dest_ip) == 0) dest_reached = 1;
				}	

			}// case while
		}//switch
	}//while

	// Calculate average packet response time
	double total_time = 0;
	for (int i = 0 ; i < 3 ; i++)
	{
		total_time += ((double)timestamps[i].tv_sec*1000) 
							+ (double)timestamps[i].tv_nsec / 1000000;
	}

	char time[30];
	sprintf(time, "%.5fms\n", total_time/3);
	strcat(reply, time);

	return dest_reached==1;
}

/*
	Check if reply string already contains given new_ip
*/
int unique_ip(char ips[][20], int count, char new_ip[20])
{	
	
	for (int i = 0 ; i < count - 1 ; i++)
	{
		if (strcmp(ips[i],new_ip) == 0)
			return 0;
	}

	strcpy(ips[count-1], new_ip);
	return 1; 
}	

/*
	Checks if received packet is a reply to us 
*/
int check_message(int ttl, struct icmphdr* icmp_header)
{
	if (icmp_header->un.echo.id != getpid()) return 0;
	if (icmp_header->un.echo.sequence != ttl) return 0;

	return 1;
}