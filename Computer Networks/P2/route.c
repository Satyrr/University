#include "route.h"

int main()
{
	/*unsigned char tab[] = { 127, 0, 5, 1 };
	unsigned char net[4];
	network_addr(tab,0,net); 
	*/
	int interface_number = 0;
	scanf("%d\n", &interface_number);
	
	Interface interfaces[interface_number];
	get_configuration(interface_number, interfaces);


	int sockfd = socket(AF_INET, SOCK_DGRAM, 0);
	int broadcastPermission = 1;
	setsockopt (sockfd, SOL_SOCKET, SO_BROADCAST,
 				(void *)&broadcastPermission,
 				sizeof(broadcastPermission));


	struct sockaddr_in server_address; 
	server_address = new_sockaddr_in(NULL, 54321);

	
	if (bind(sockfd, 
			(struct sockaddr*)&server_address, 
			sizeof(server_address)) < 0) {
		fprintf(stderr, "bind error: %s\n", strerror(errno)); 
		return EXIT_FAILURE;
	}

	Vector v;
	v.number = 0;

	// fill vector with neighbour networks
	init_vector(&v, interfaces, interface_number);

	for(;;)
	{
		// send control messages to neighbour networks
		neighbour_messages(sockfd, interfaces, interface_number);

		//receive packets
		if(receive_vectors(&v, sockfd, interfaces, interface_number) 
			== EXIT_FAILURE)
			return EXIT_FAILURE;

		show_vector(&v);

		//send vector to all neighbours
		send_vector(sockfd, interfaces, interface_number, &v);
	}

	return 0;
}