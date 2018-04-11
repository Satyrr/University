#include <avr/io.h>
#include <stdio.h>
#include <string.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include <avr/sleep.h>

/* uart */

#define BAUD 9600                          // baudrate
#define UBRR_VALUE ((F_CPU)/16/(BAUD)-1)   // zgodnie ze wzorem

// inicjalizacja UART
void uart_init()
{
  // ustaw baudrate
  UBRR0 = UBRR_VALUE;
  // włącz odbiornik i nadajnik
  UCSR0B = _BV(RXEN0) | _BV(TXEN0) | _BV(RXCIE0);
  // ustaw format 8n1
  UCSR0C = _BV(UCSZ00) | _BV(UCSZ01);
}

/* End of uart */

int main() {

  uart_init();
  sei();
  PRR = 0xed; // shutdown all but UART
  
  while (1) {
	set_sleep_mode(SLEEP_MODE_IDLE);
 	sleep_mode();
  }
}


ISR (USART_RX_vect)  
{
  char ReceivedByte;
  ReceivedByte = UDR0; 
  UDR0 = ReceivedByte; 
}

