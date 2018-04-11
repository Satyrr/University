objects=main.o icmp_send.o icmp_checksum.o pick_msg.o
compile=gcc -Wall -Wextra -std=gnu99

main: $(objects)
	$(compile) -o traceroute $(objects)

main.o: main.c traceroute.h
	$(compile) -c main.c
icmp_checksum.o: icmp_checksum.c traceroute.h
	$(compile) -c icmp_checksum.c
icmp_send.o: icmp_send.c traceroute.h
	$(compile) -c icmp_send.c
pick_msg.o: pick_msg.c traceroute.h
	$(compile) -c pick_msg.c

clean:
	rm $(objects)
distclean:
	rm traceroute $(objects)