#include "transport.h"

int main(int argc, char* argv[])
{
	if (argc != 4) 
	{
		fprintf(stderr, "usage: %s port filename size\n", argv[0]);
		return EXIT_FAILURE;
	}

	int sockfd = socket(AF_INET, SOCK_DGRAM, 0);
	if (sockfd < 0) 
	{
		fprintf(stderr, "socket error: %s\n", strerror(errno)); 
		return EXIT_FAILURE;
	}
	
	int port_number = atoi(argv[1]);
	if (port_number == 0)
	{
		printf("Invalid port number");
		return EXIT_FAILURE;
	}

	char filename[strlen(argv[2])];
	strcpy(filename, argv[2]);

	int size = atoi(argv[3]);
	if (size == 0)
	{
		printf("Invalid size");
		return EXIT_FAILURE;
	}

	struct sockaddr_in server_address;
	bzero (&server_address, sizeof(server_address));
	server_address.sin_family = AF_INET;
	server_address.sin_port = htons(port_number);
	inet_pton(AF_INET, "156.17.4.30", &server_address.sin_addr);

	FILE *fp;
	if ((fp = fopen(filename, "w")) == NULL) 
	{
		printf ("Can't create file\n");
		exit(1);
	}

	Window window;
	bzero(&window, sizeof(window));

	int saved_bytes = 0;
	while (size > saved_bytes)
	{
		send_requests(size, &window, sockfd, &server_address);

		get_messages(sockfd, &window, size, port_number);

		int res = update_window(&window, fp);
		saved_bytes += res;
		if(res > 0) printf("%.3f%%\n", 100*((double)saved_bytes/size));

	}

	close(sockfd);
	fclose(fp);

	return EXIT_SUCCESS;
}