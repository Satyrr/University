#include "transport.h"

int ceiling_div(int x, int y)
{
	return (x + y - 1) / y;
}

int read_datagram(char *datagram,
	int *start,
	int *length,
	char **data)
{
	int s, l;
	int res = sscanf(datagram, "DATA %d %d\n", &s, &l);

	if(res < 2) return INVALID_DATAGRAM;

	*start = s;
	*length = l;
	*data = strchr(datagram, '\n') + 1;

	return 0;
}

int is_datagram_foreign(int start, int len, int size)
{
	if(start % SEGMENT_SIZE != 0) return 1;
	if(len > SEGMENT_SIZE) return 1;
	if(len < SEGMENT_SIZE && start + len < size) return 1;
	if(start + len > size) return 1;

	return 0;
}
