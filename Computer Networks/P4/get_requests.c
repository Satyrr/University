#include "server.h"

int read_request(int sockfd, struct timeval* tv,
				Buffer* req_buf, Request* req, char* server_catalog)
{
	int newlines_in_a_row = 0; // header ends with 4 '\n' / '\r' in a row
	char line[4096];
	int line_chars = 0;

	//read full request
	while(newlines_in_a_row < 4)
	{
		if(line_chars >= BUFFSIZE - 1) return 0;
		// buffer readen - fill with next bytes
		if(req_buf->buffer_readen == req_buf->buffer_len)
		{
			int res = fill_buffer(sockfd, tv, req_buf);
			if(res <= 0) return res;
		}
		//read next char
		char c = req_buf->buffer[req_buf->buffer_readen++];
		if(c == '\n')
		{
			line[line_chars] = '\n';
			line[line_chars + 1] = 0;
			//printf("%s", line);
			parse_line(line, req);
			newlines_in_a_row++;
			line_chars = 0;
		} 
		else if(c == '\r')
			newlines_in_a_row++;
		else
		{
			line[line_chars++] = c;
			newlines_in_a_row = 0;
		}

	}

	delete_ending_slashes(req->path);
	delete_ending_slashes(req->host);

	//traversal attack
	if( check_realpath(server_catalog, req->host, req->path) != 0 )
		req->get_flag = 0; 

	return 1;
}

int fill_buffer(int sockfd, struct timeval* tv, Buffer* req_buf)
{
	req_buf->buffer_len = req_buf->buffer_readen = 0;
	fd_set descriptors;
	FD_ZERO (&descriptors);
	FD_SET (sockfd,&descriptors);

	int ready = Select(sockfd+1, &descriptors, NULL, NULL, tv);
	if(ready <= 0) 
		return ready;

	req_buf->buffer_len = Recv(sockfd, req_buf->buffer, BUFFSIZE, 0);	

	return req_buf->buffer_len;

}

void parse_line(char *line, Request* req)
{
	if(strlen(line) < 5) return;

	if(strncmp(line, "GET ", 4) == 0)
	{
		char* get_line = line+4;

		char* version = strchr(get_line, ' ');
		if(version == NULL) return;

		int pathlen = strlen(get_line) - strlen(version);

		if(pathlen >= 500) return;

		strncpy(req->path, get_line, pathlen);
		strncpy(req->version, version, strlen(version)-1);

		req->version[strlen(version)-1] = 0;
		req->path[pathlen] = 0;

		req->get_flag = 1;
	}
	else if(strncmp(line, "Host: ", 6) == 0)
	{
		char* get_line = line+6;
		char* port = strchr(get_line, ':');
		if(port == NULL) return;

		int hostlen = strlen(get_line) - strlen(port);

		if(hostlen >= 250) return;

		strncpy(req->host, get_line, hostlen);
		req->host[hostlen] = 0;

		req->host_flag = 1;
	}
	else if(strncmp(line, "Connection: close", strlen("Connection: close")))
		req->connection_close = 1;

}