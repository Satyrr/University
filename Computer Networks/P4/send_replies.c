#include "server.h"


int send_reply(int sockfd, Request* req, char* server_catalog, int port)
{
	//invalid request
	if(req->get_flag == 0 || req->host_flag == 0)
	{
		send_error_reply(sockfd, 501, req);
		return CLOSE_CONNECTION;
	}
	//invalid host
	if(check_host(server_catalog, req) == NOT_EXISTS)
	{
		send_error_reply(sockfd, 404, req);
		return CLOSE_CONNECTION;
	}
	//invlaid url
	if(check_URL(server_catalog, req) == NOT_EXISTS)
	{
		send_error_reply(sockfd, 404, req);
		return CLOSE_CONNECTION;
	}
	//if folder requested - redirect
	if(check_URL(server_catalog, req) == ISFOLDER)
	{	
		send_moved_reply(sockfd, req, port);
		return KEEP_CONNECTION;
	}
	//200 ok
	if(check_URL(server_catalog, req) == ISFILE)
	{
		send_ok_reply(sockfd, server_catalog, req);
	}

	return KEEP_CONNECTION;

}

void send_error_reply(int sockfd, int code_nr, Request *req)
{
	char code[70];
	char content[200];
	switch(code_nr)
	{
		case 403:
			sprintf(code,"403 Forbidden");
			sprintf(content, "<h1>Forbidden</h1> \
							<p>You don't have permission to access this recource.</p>" );
			break;
		case 404:
			sprintf(code,"404 Not Found");
			sprintf(content, "<h1>Not Found</h1> \
  							<p>URL not found.</p>" );
			break;
		case 501:
			sprintf(code,"501 Not Implemented");
			sprintf(content, "<h1>Not Implemented</h1> \
  							<p>Invalid request or not implemented function.</p>" );
			break;
		default:
			return;
	}

	char length[70];
	int len = strlen(content);
	sprintf(length, "Content-Length: %d", len);

	reply(sockfd,
		code,
		length,
		"Content-Type: text/html",
		content,
		len,
		req->version);
}

void send_ok_reply(int sockfd, char *server_catalog, Request* req)
{
	char path[300];
	sprintf(path, "%s/%s/%s", server_catalog, req->host, req->path);
	delete_multi_slashes(path);

	char type[80], content_type[100];
	enum file_type ft = file_type(req->path, type);
	sprintf(content_type, "Content-Type: %s", type);

	int len = 0;
	char *content;
	content = load_file(path, ft, &len);

	char length[70];
	sprintf(length, "Content-Length: %d", len);

	reply(sockfd,
		"200 OK",
		length,
		content_type,
		content,
		len,
		req->version);

	free(content);
}

void send_moved_reply(int sockfd, Request* req, int port)
{

	char path[250];
	sprintf(path ,"%s:%d/%s/index.html", req->host, port, req->path);

	delete_multi_slashes(path);

	char location[300];
	sprintf(location, "Location: http://%s", path);

	reply(sockfd,
		"301 Moved Permanently",
		"",
		"",
		location,
		strlen(location),
		req->version);
}

int reply(int sockfd, char* code, char* length, char* type,
		 char* content, int content_len, char* version)
{
	int msglen = strlen(code) + strlen(length) + strlen(version) +
				strlen(type) + content_len;

	char *message = malloc(msglen + 10);

	size_t nleft;
	
	if(strcmp(code, "301 Moved Permanently") == 0)
	{
		sprintf(message, "%s %s\n%s\n\n", version, code, content);
		nleft = strlen(message);
	}
	else
	{
		sprintf(message, "%s %s\n%s\n%s\n\n", version, code, 
			length, type);

		int header_len = strlen(message);
		memcpy(message+header_len, content, content_len);

		nleft = header_len + content_len;
	}
	char *buff_pointer = message; 

	while (nleft  > 0) {
		int result = send(sockfd, buff_pointer, nleft, 0);
		if (result < 0) 
		{
			free(message);
			fprintf(stderr, "%s: %s\n", "send error", strerror(errno));
			return 0;
		}
		nleft -= result;
		buff_pointer += result;
	} 

	free(message);
	return 1;
}