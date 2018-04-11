#include "route.h"

// gets computer interfaces configuration from stdin
void get_configuration(int number, Interface* interfaces)
{
	for(int idx = 0; idx < number; idx++)
	{
		char line[50];
		char ip_string[25];
		char mask_string[10];
		char distance_string[30];

		fgets(line, 50, stdin);

		int i = 0, j = 0, k = 0;

		while(line[i] != '/')
		{
			ip_string[i] = line[i];
			i++;
		}

		ip_string[i] = '\0';

		while(line[i] != ' ')
		{
			i++;
			mask_string[j] = line[i];
			j++;
		}
		mask_string[j] = '\0';

		i++;
		while(line[i] != ' ')
			i++;
		i++;

		while(line[i] >= '0' && line[i] <= '9')
		{
			distance_string[k] = line[i];
			i++; k++;
		}
		distance_string[k] = '\0';

		interfaces[idx].mask = atoi(mask_string);
		interfaces[idx].distance = atoi(distance_string);

		string_to_bytes_ip(ip_string, interfaces[idx].ip_bytes);
	}
}

// creates new sockaddr_in with given port
struct sockaddr_in new_sockaddr_in(unsigned char* ip, short port)
{
	struct sockaddr_in server_address;
	bzero (&server_address, sizeof(server_address));
	server_address.sin_family = AF_INET;
	server_address.sin_port = htons(port);

	if(ip!=NULL){
		char string_ip[20];
		sprintf(string_ip, 
			"%d.%d.%d.%d",
			(unsigned int)ip[0],
			(unsigned int)ip[1],
			(unsigned int)ip[2],
			(unsigned int)ip[3]);
		inet_pton(AF_INET, (char*)string_ip, &server_address.sin_addr);
	}
	else
		server_address.sin_addr.s_addr = htonl(INADDR_ANY);

	return server_address;
}

// computes broadcast addr of ip[4], saves it to broadcast_ip[4]
void broadcast_addr(unsigned char ip[4],
	unsigned char m,
	unsigned char broadcast_ip[4])
{
	unsigned int mask = (unsigned int)m;
	mask = 0xffffffff>>mask;

	broadcast_ip[0] = ip[0] | ((mask & 0xff000000)>>24);
	broadcast_ip[1] = ip[1] | ((mask & 0x00ff0000)>>16);
	broadcast_ip[2] = ip[2] | ((mask & 0x0000ff00)>>8);
	broadcast_ip[3] = ip[3] | (mask & 0x000000ff);
}

// computes network addr of ip[4], saves it to network_ip[4]
void network_addr(unsigned char ip[4],
	unsigned char m,
	unsigned char network_ip[4]){

	unsigned int mask = (unsigned int)m;
	mask = 0xffffffff << (32 - mask);
	if(m == 0) mask = 0;

	network_ip[0] = ip[0] & ((mask & 0xff000000)>>24);
	network_ip[1] = ip[1] & ((mask & 0x00ff0000)>>16);
	network_ip[2] = ip[2] & ((mask & 0x0000ff00)>>8);
	network_ip[3] = ip[3] & (mask & 0x000000ff);
}

void string_to_bytes_ip(
	char* ip_string,
	unsigned char ip_bytes[]
	){

	uint32_t ip;
	inet_pton(AF_INET, (char*)ip_string, &ip);
	ip = ntohl(ip);

	ip_bytes[0] = ((ip & 0xff000000) >> 24) & 0x000000ff;
	ip_bytes[1] = ((ip & 0x00ff0000) >> 16) & 0x000000ff;
	ip_bytes[2] = ((ip & 0x0000ff00) >> 8) & 0x000000ff;
	ip_bytes[3] = (ip & 0x000000ff) & 0x000000ff;
}

void int_to_bytes_ip(
	uint32_t ip,
	unsigned char ip_bytes[4]
	){

	ip_bytes[0] = ((ip & 0xff000000) >> 24) & 0x000000ff;
	ip_bytes[1] = ((ip & 0x00ff0000) >> 16) & 0x000000ff;
	ip_bytes[2] = ((ip & 0x0000ff00) >> 8) & 0x000000ff;
	ip_bytes[3] = (ip & 0x000000ff) & 0x000000ff;
}