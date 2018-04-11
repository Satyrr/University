//Marcin Gruza
//281050
#include "traceroute.h"

int send_icmp_request(int sockfd, char *ip, int ttl)
{
	/*
		ICMP header declaration.
		Unique id of packet is process id.
		clock() is used as timestamp.
	*/
	struct icmphdr icmp_header;
	icmp_header.type = ICMP_ECHO;
	icmp_header.code = 0;
	icmp_header.un.echo.id = getpid();
	icmp_header.un.echo.sequence = ttl;
	icmp_header.checksum = 0;
	icmp_header.checksum = compute_icmp_checksum (
		(u_int16_t*)&icmp_header, sizeof(icmp_header));

	/*
		Recipient declaration.
	*/
	struct sockaddr_in recipient;
	bzero (&recipient, sizeof(recipient));
	recipient.sin_family = AF_INET;
	inet_pton(AF_INET, ip, &recipient.sin_addr);

	/*
		Set ttl of packet
	*/
	setsockopt (sockfd, IPPROTO_IP, IP_TTL, &ttl, sizeof(int));

	/*
		Send packet
	*/
	ssize_t bytes_sent = sendto (
	 sockfd,
	 &icmp_header,
	 sizeof(icmp_header),
	 0,
	 (struct sockaddr*)&recipient,
	 sizeof(recipient)
	);

	return bytes_sent;
}