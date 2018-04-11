objects=server.o utils.o sockwrap.o get_requests.o send_replies.o 
compile=gcc -g -Wall -Wextra -std=gnu99

server: $(objects)
	$(compile) -o server $(objects)

server.o: server.c server.h
	$(compile) -c server.c
get_requests.o: get_requests.c server.h
	$(compile) -c get_requests.c
send_replies.o: send_replies.c server.h
	$(compile) -c send_replies.c
utils.o: utils.c server.h
	$(compile) -c utils.c
sockwrap.o: sockwrap.c server.h
	$(compile) -c sockwrap.c

clean:
	rm -f $(objects)
distclean:
	rm -f server $(objects)