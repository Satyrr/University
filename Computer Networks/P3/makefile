objects=utils.o transport.o messages.o
compile=gcc -g -Wall -Wextra -std=gnu99

transport: $(objects)
	$(compile) -o transport $(objects)

transport.o: transport.c transport.h
	$(compile) -c transport.c
messages.o: messages.c transport.h
	$(compile) -c messages.c
utils.o: utils.c transport.h
	$(compile) -c utils.c

clean:
	rm -f $(objects)
distclean:
	rm -f transport $(objects)