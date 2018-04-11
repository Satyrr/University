#include "transport.h"

#define GET_TIMEOUT 500000
#define SELECT_TIMEOUT 100

//sends single message "GET start length\n"
void send_request(
	int start,
	int length,
	int sockfd,
	struct sockaddr_in *server_address)
{
	//message
	char message[50];
	sprintf(message, "GET %d %d\n", start, length);
	int message_len = strlen(message);

	if (sendto(sockfd,
		message, 
		message_len,
		0, 
		(struct sockaddr*) server_address, 
		sizeof(*server_address)) != message_len) 
	{
			fprintf(stderr, "sendto error: %s\n", strerror(errno)); 
			exit(1);		
	}
}

//sends get message for timeouted segments
void send_requests(
	int size,
	Window *window,
	int sockfd,
	struct sockaddr_in *server_address)
{
	int total_segments = ceiling_div(size, SEGMENT_SIZE);

	//current time
	struct timespec now;
	clock_gettime(CLOCK_REALTIME, &now); 

	for (int i = 0; i < WINDOW_SIZE && window->first_segment + i < total_segments; i++)
	{
		int index = (i + window->first_segment)%WINDOW_SIZE;
		Segment *seg = &window->segments[index];

		if (seg->received == 1) continue;
		if (now.tv_nsec - seg->time.tv_nsec > GET_TIMEOUT ||
			now.tv_sec - seg->time.tv_sec > 0)
		{
			int start = (window->first_segment + i)*SEGMENT_SIZE;

			int length = SEGMENT_SIZE;
			if (start + length > size)
				length = size - start;

			send_request(start, length, sockfd, server_address);
			clock_gettime(CLOCK_REALTIME, &(seg->time)); 
		}
	}

}

void get_messages(int sockfd, Window* window, int size, int port_number)
{
	char buffer[IP_MAXPACKET+1];

	struct sockaddr_in sender;	
	socklen_t sender_len = sizeof(sender);

	fd_set descriptors;
	FD_ZERO (&descriptors);
	FD_SET (sockfd, &descriptors);

	struct timeval tv; tv.tv_sec = 0; tv.tv_usec = SELECT_TIMEOUT;

	int ready = select(sockfd+1, &descriptors, NULL, NULL, &tv);
	if (ready == -1)
	{
		fprintf(stderr, "select error: %s\n", strerror(errno)); 
		exit(1);
	}
	while (ready == 1)
	{
		ssize_t datagram_len = 0;
		datagram_len = recvfrom(sockfd,
								buffer, 
								IP_MAXPACKET, 
								MSG_DONTWAIT, 
		                         (struct sockaddr*)&sender, 
		                         &sender_len);
		if (datagram_len < 0) break;

		char sender_ip_str[20]; 
		inet_ntop(AF_INET,
			&(sender.sin_addr),
			sender_ip_str,
		 	sizeof(sender_ip_str));

		if (strcmp(sender_ip_str, "156.17.4.30") != 0) continue;
		if (port_number != ntohs(sender.sin_port)) continue;

		buffer[datagram_len] = 0;

		char *data;
		int start, length;

		if (read_datagram(buffer,
			&start,
			&length,
			&data) == INVALID_DATAGRAM )
			continue;

		if(is_datagram_foreign(start, length, size)) continue;

		int segment_number = start/SEGMENT_SIZE;
		if (segment_number < window->first_segment ) continue;

		receive_data(start, length, data, window);
		ready = select (sockfd+1, &descriptors, NULL, NULL, &tv);
		if (ready == -1)
		{
			fprintf(stderr, "select error: %s\n", strerror(errno)); 
			exit(1);
		}
	}	
	
}

void receive_data(
	int start,
	int length,
	char* data,
	Window* window)
{
	int segment_number = start/SEGMENT_SIZE;
	int index = segment_number % WINDOW_SIZE;

	window->segments[index].received = 1;
	window->segments[index].len = length;

	memcpy(window->segments[index].buffer, data, length);
}

int update_window(Window* window,
	FILE* fp)
{
	int saved = 0;
	int updated = 0;
	while (updated != 1)
	{
		int index = window->first_segment % WINDOW_SIZE;
		Segment seg = window->segments[index];

		//checks if first segment has been received
		if (seg.received == 1)
		{
			int length = window->segments[index].len;
			if (fwrite((void*)window->segments[index].buffer,
				1,
				length,
				fp) != (unsigned int)length)
			{
				fprintf(stderr, "fwrite error\n"); 
				exit(1);
			}

			//resets segment
			bzero(&window->segments[index], sizeof(window->segments[index]));

			window->first_segment++;
			saved += length;
		}
		else
			updated = 1;
	}
	return saved;
}