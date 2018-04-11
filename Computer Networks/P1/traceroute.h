#ifndef _TRACEROUTE_H_
#define _TRACEROUTE_H_

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <errno.h>
#include <time.h>
#include <unistd.h>
#include <stdint.h>
#include <assert.h>
#include <math.h>

#include <netinet/ip.h>
#include <netinet/ip_icmp.h>
#include <arpa/inet.h>


int send_icmp_request(int sockfd, char *ip, int ttl);
u_int16_t compute_icmp_checksum (const void *buff, int length);
int pick_messages(int sockfd, struct timespec timestamps[3], 
					char* reply, char* dest_ip, int ttl);
int unique_ip(char ips[][20], int count, char new_ip[20]);
int check_message(int ttl, struct icmphdr* icmp_header);

#endif