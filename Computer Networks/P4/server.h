#ifndef __SERVER_H_
#define __SERVER_H_

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <arpa/inet.h>
#include <errno.h>
#include <unistd.h>
#include <dirent.h> 
#include <time.h>

#define ERROR(str) { fprintf(stderr, "%s: %s\n", str, strerror(errno)); exit(1); }
#define BUFFSIZE 4096

#define REC_TIMEOUT 0
#define CONNECTION_CLOSE -1

#define CLOSE_CONNECTION 0
#define KEEP_CONNECTION 1

#define NOT_EXISTS 0
#define ISFOLDER 1
#define ISFILE 2

enum file_type{
	byte,
	text
};

typedef struct {
	char buffer[BUFFSIZE];
	int buffer_len;
	int buffer_readen;
} Buffer;

typedef struct {
	char path[550];
	char host[300];
	char version[100];
	int connection_close;
	int get_flag, host_flag;
} Request;

//sockwrap.c
int Socket(int family, int type, int protocol);
void Bind(int fd, const struct sockaddr_in *sa, socklen_t salen);
void Connect(int fd, const struct sockaddr_in *sa, socklen_t salen);
int Accept(int fd, struct sockaddr_in *sa, socklen_t *salenptr);
void Listen(int fd, int backlog);
ssize_t Recv(int fd, char *ptr, size_t nbytes, int flags);
void Close(int fd);
int Select(int nfds, fd_set *readfds, fd_set *writefds,
		 fd_set *exceptfds, struct timeval *timeout);


//get_requests.c
int read_request(int sockfd, struct timeval* tv,
				Buffer* req_buf, Request* req, char* serv_catalog);
int fill_buffer(int sockfd, struct timeval* tv, Buffer* req_buf);
void parse_line(char *line, Request* req);

//send_reply.c
int send_reply(int sockfd, Request* req, char* server_catalog, int port);
void send_error_reply(int sockfd, int code_nr, Request *req);
void send_ok_reply(int sockfd, char *server_catalog, Request* req);
void send_moved_reply(int sockfd, Request* req, int port);
int reply(int sockfd, char* code, char* length, char* type,
		 char* content, int content_len, char* version);
//utils.c 
char* load_file(char *path, enum file_type ft, int* len);
int check_URL(char* server_catalog, Request* req);
int check_host(char* server_catalog, Request* req);
enum file_type file_type(char* path, char* type);
void delete_ending_slashes(char *str);
void delete_multi_slashes(char *str);
int check_realpath(char* catalog, char* host, char* path);
void init_request(Request* req);

#endif

