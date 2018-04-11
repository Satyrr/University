#ifndef _ROUTE_H_
#define _ROUTE_H_

#include <netinet/ip.h>
#include <arpa/inet.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <errno.h>

#define NOT_NEIGHBOUR 0
#define NEIGHBOUR 1

#define NOT_FOUND -1
#define DIRECTLY_CON -1
#define SUCCESS 0;
#define INFINITY_DIST 0xffffffff

typedef struct interface
{
	unsigned char ip_bytes[4];
	unsigned char mask;
	int distance;
} Interface;

typedef struct vector_element Vector_element;

struct vector_element
{
	unsigned char ip_bytes[4];
	unsigned char mask;
	unsigned int distance;
	int is_neighbour;
	unsigned char via_ip[4];
	int no_respond_turns;
};

typedef struct vector
{
	Vector_element elements[50];
	int number;
} Vector;

//route_utils.c
void get_configuration (
	int number, 
	Interface* interfaces
	);
struct sockaddr_in new_sockaddr_in (
	unsigned char* ip,
	short port
	);
void broadcast_addr (
	unsigned char ip[4],
	unsigned char mask,
	unsigned char broadcast_ip[4]
	);
void network_addr (
	unsigned char ip[4],
	unsigned char mask,
	unsigned char network_ip[4]
	);
void string_to_bytes_ip (
	char* ip_string,
	unsigned char ip_bytes[4]
	);
void int_to_bytes_ip(
	uint32_t ip,
	unsigned char ip_bytes[4]
	);

//interfaces.c
int own_ip (
	unsigned char ip[4], 
	Interface* interfaces,
	int interface_number
	);

int own_network (
	unsigned char ip[4], 
	Interface* interfaces,
	int interface_number
	);
void interface_unreachable_update (
	Interface* in,
	int index,
	Vector *v);
void neighbour_messages (
	int sockfd,
	Interface* interfaces,
	int interface_number
	);
unsigned int neighbour_distance(Interface* in,
	int in_number,
	unsigned char* neigh_ip);

//vector.c
void show_vector (Vector* v);
void init_vector (
	Vector *v,
	Interface *interfaces,
	int interface_number
	);
int find_element_index (
	Vector *v, 
	unsigned char ip[4], 
	unsigned char mask
	);
void new_element (
	Vector *v,
	unsigned char ip[4],
	unsigned char mask,
	unsigned int distance,
	int is_neighbour,
	unsigned char via_ip[4]
	);
int send_vector (int sockfd,
	Interface* in,
	int in_number,
	Vector *v
	);
int receive_vectors(Vector *v,
					int sockfd,
					Interface* interfaces,
					int interface_number
					);
void update_no_respond (Vector *v);
 
#endif
