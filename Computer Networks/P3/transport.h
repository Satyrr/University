#ifndef _TRANSPORT_H
#define _TRANSPORT_H

#include <netinet/ip.h>
#include <arpa/inet.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <errno.h>
#include <time.h>

#define WINDOW_SIZE 100
#define SEGMENT_SIZE 1000

#define INVALID_DATAGRAM -1

typedef struct segment{
	char buffer[SEGMENT_SIZE];
	char received;
	int len;
	struct timespec time;
} Segment;

typedef struct window{
	Segment segments[WINDOW_SIZE];
	int first_segment;
} Window;

//messages.c
void send_requests(int size,
	Window *window,
	int sockfd,
	struct sockaddr_in* serv_addr);

void receive_data(int start,
	int length,
	char* data,
	Window* window);

int update_window(Window* window,
	FILE* fp);

void get_messages(int sockfd,
	Window* window,
 	int size,
 	int port_number);

//utils.c
int ceiling_div(int x, int y);

int read_datagram(char *datagram,
	int *start,
	int *length,
	char **data);

int is_datagram_foreign(int start,
	int len,
	int size);

#endif