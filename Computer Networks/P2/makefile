objects=route.o route_utils.o vector.o interfaces.o
compile=gcc -g -Wall -Wextra -std=gnu99

route: $(objects)
	$(compile) -o route $(objects)

route.o: route.c route.h
	$(compile) -c route.c
route_utils.o: route_utils.c route.h
	$(compile) -c route_utils.c
vector.o: vector.c route.h
	$(compile) -c vector.c
interfaces.o: interfaces.c route.h
	$(compile) -c interfaces.c

clean:
	rm $(objects)
distclean:
	rm route $(objects)