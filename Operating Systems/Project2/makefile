all : test1 test2

test1: filesystem.c disk.c test1.c disk.h filesystem.h
	gcc -Wall -Werror test1.c filesystem.c disk.c -o test1

test2: filesystem.c disk.c test2.c disk.h filesystem.h
	gcc -Wall -Werror test2.c filesystem.c disk.c -o test2

clean: 
	rm -f test1 test2 dysk1
