#include "server.h"

char* load_file(char *path, enum file_type ft, int* len)
{
	char mode[5];
	if(ft == byte)
		strcpy(mode, "r");
	else
		strcpy(mode, "rb");

	char *content = NULL;
	long length = 0;

	FILE *f;
	f = fopen(path, mode);

	if (f)
	{
		fseek(f, 0, SEEK_END);
		length = ftell(f);
		*len = length;

		fseek(f, 0, SEEK_SET);

		content = malloc (length + 1);
		if (content)
		{
			fread(content, 1, length, f);
			content[length] = 0;
		}
		fclose (f);
	}
	return content;
}

enum file_type file_type(char* path, char* type)
{
	char* ext = strrchr(path, '.');
	if(ext == NULL)
		ext = "no_ext";
	enum file_type ft = byte;

	if(strncmp(ext, ".txt", 4) == 0)
	{
		sprintf(type, "text/plain");
		ft = text;
	}
	else if(strncmp(ext, ".html", 5) == 0)
	{
		sprintf(type, "text/html");
		ft = text;
	}
	else if(strncmp(ext, ".css", 4) == 0)
	{
		ft = text;
		sprintf(type, "text/css");
	}
	else if(strncmp(ext, ".jpg", 4) == 0)
		sprintf(type, "image/jpeg");
	else if(strncmp(ext, ".jpeg", 5) == 0)
		sprintf(type, "image/jpeg");
	else if(strncmp(ext, ".png", 4) == 0)
		sprintf(type, "image/png");
	else if(strncmp(ext, ".pdf", 4) == 0)
		sprintf(type, "application/pdf");
	else
		sprintf(type, "application/octet-stream");

	return ft;
}

//checks whether request url points to either file or folder
int check_URL(char* server_catalog, Request* req)
{
 	char path[200];
 	sprintf(path, "./%s/%s/%s", server_catalog, req->host, req->path);

 	int path_valid = NOT_EXISTS;

 	DIR *d;
 	d = opendir(path);
 	FILE *file;
	file = fopen(path, "r");

 	if(d)
 		path_valid = ISFOLDER;
 	else if(file != NULL)
 		path_valid = ISFILE;

 	closedir(d);
 	if(file != NULL) fclose(file);

	return path_valid;
}

//checks if host exists
int check_host(char* server_catalog, Request* req)
{
 	char host_catalog[200];
 	sprintf(host_catalog, "./%s/%s", server_catalog, req->host);

 	DIR *d;
 	int host_valid = 0;
 	d = opendir(host_catalog);
 	if(d) host_valid = 1;
 	closedir(d);

	return host_valid;
}

void delete_ending_slashes(char *str)
{
	int len = strlen(str);

	while(len > 0 && str[len-1] == '/')
	{
		str[len-1] = 0;
		len--;
	}
}

void delete_multi_slashes(char *str)
{
	int len = strlen(str);

	char temp[len];
	strncpy(temp, str, len);	

	int i, k = 0, slash_nr = 0;
	for(i = 0; i < len; i++)
	{
		if(temp[i] == '/')
			slash_nr++;
		else
			slash_nr = 0;

		if(slash_nr < 2)
		{
			str[k] = temp[i];
			k++;
		}
	}

	str[k] = 0;
}

void init_request(Request* req)
{
	req->connection_close = 0;
	req->get_flag = 0;
	req->host_flag = 0;
	strcpy(req->path, "");
	strcpy(req->host, "");
}

int check_realpath(char* catalog, char* host, char* path)
{
	char relative[PATH_MAX], absolute[PATH_MAX];
	sprintf(relative, "./%s/%s/%s", catalog, host, path);

	realpath(relative, absolute);

	char base_dir_relative[PATH_MAX], base_dir_absolute[PATH_MAX];
	sprintf(base_dir_relative, "./%s/%s", catalog, host);

	realpath(base_dir_relative, base_dir_absolute);

	return strncmp(base_dir_absolute, absolute, strlen(base_dir_absolute));
}