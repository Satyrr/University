#include "server.h"
#include <time.h>


int main (int argc, char *argv[])
{

	if (argc < 2) {
    	fprintf(stderr, "Two parameters required: %s port catalog\n", argv[0]);
    	exit(-1);
  	}

	int port = atoi (argv[1]);
	if ( port <= 0 || port > 65535)
	{
		printf("Bad port number\n");
		exit(-1);
	}

	delete_ending_slashes(argv[2]);
	DIR *d;
 	d = opendir(argv[2]);
	// check if folder exists
	if (!d) {
		fprintf(stderr, "Folder doesn't exist: %s\n", argv[2]);
		exit(-1);
   	}
   	closedir(d);

	int sockfd = Socket (AF_INET, SOCK_STREAM, 0);

	struct sockaddr_in server_address;
	bzero(&server_address, sizeof(server_address));
	server_address.sin_family      = AF_INET;
	server_address.sin_port        = htons(port);
	server_address.sin_addr.s_addr = htonl(INADDR_ANY);

	Bind(sockfd, &server_address, sizeof(server_address));

	Listen(sockfd, 64);

	while (1)
	{
		struct sockaddr_in client_address;
		socklen_t len = sizeof(client_address);
		int conn_sockfd = Accept(sockfd, &client_address, &len);

		char ip_address[20];
		inet_ntop (AF_INET, &client_address.sin_addr, ip_address, sizeof(ip_address));
		printf ("\nNew client %s:%d\n\n", ip_address, ntohs(client_address.sin_port));

		Buffer req_buf;
		req_buf.buffer_len = req_buf.buffer_readen = 0;
		struct timeval tv; tv.tv_sec = 1; tv.tv_usec = 0;

		int ready = 1;
		while(ready > 0)
		{
			Request req; 
			init_request(&req);

			ready = read_request(conn_sockfd, &tv, &req_buf, &req, argv[2]);

			if(ready <= 0) break; // if timeout or error
			
			ready = send_reply(conn_sockfd, &req, argv[2], port);

			if(ready == KEEP_CONNECTION) // if sent successfully, reset timer
			{
				tv.tv_sec = 1; tv.tv_usec = 0;
			}
			else if(ready == CONNECTION_CLOSE)
				break;

		}

		Close (conn_sockfd);
	}


	

	return 0;
}
