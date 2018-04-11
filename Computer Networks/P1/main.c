//Marcin Gruza
//281050
#include "traceroute.h"

int check_ip(char* ip)
{
	struct sockaddr_in sa;
    int result = inet_pton(AF_INET, ip, &(sa.sin_addr));
    return result != 0;
}

int main(int argc, char* argv[])
{

	if (argc < 2)
	{
		printf("One parameter required.\n");
		return 0;
	}

	char *ip = argv[1];

	if (check_ip(ip) < 0)
	{
		printf("Invalid argument\n");
		return EXIT_FAILURE;
	}
	// Open socket
	int sockfd = socket(AF_INET, SOCK_RAW, IPPROTO_ICMP);
	if (sockfd < 0) 
	{
		fprintf(stderr, "socket error: %s\n", strerror(errno)); 
		return EXIT_FAILURE;
	}

	char result[3000];
	strcpy(result, "");

	// Send-get loop
	for (int ttl = 1; ttl <= 30; ttl++) 
	{

		// Send icmp packets
		struct timespec timestamps[3]; // Sum of reply times
		for (int i = 0;i < 3 ; i++)
		{
			if (send_icmp_request(sockfd, ip, ttl) == -1)
			{
				fprintf(stderr, "send packet error: %s\n", strerror(errno)); 
				return EXIT_FAILURE;
			} 
			// Make timestamp
   			clock_gettime(CLOCK_REALTIME, &timestamps[i]);
		}

		// Get reply packets
		char reply[100];
		strcpy(reply, "");

		int res = pick_messages(sockfd, timestamps, reply, ip, ttl);
		// res == -1 - error
		if (res == -1 ) return EXIT_FAILURE;

		//print reply
		printf("%d. %s",ttl, reply);

		// res == 1 - destination reached
		if (res == 1) ttl = 31;
	}


	return EXIT_SUCCESS;
}