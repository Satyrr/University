#include <avr/io.h>
#include <stdio.h>
#include <string.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include <avr/sleep.h>

#define LED PB1
#define LED_DDR DDRB
#define LED_PORT PORTB

/* uart */

#define BAUD 9600                          // baudrate
#define UBRR_VALUE ((F_CPU)/16/(BAUD)-1)   // zgodnie ze wzorem

// inicjalizacja UART
void uart_init()
{
  // ustaw baudrate
  UBRR0 = UBRR_VALUE;
  // włącz odbiornik i nadajnik
  UCSR0B = _BV(RXEN0) | _BV(TXEN0) | _BV(TXCIE0);
  // ustaw format 8n1
  UCSR0C = _BV(UCSZ00) | _BV(UCSZ01);
}
int uart_transmit();

void timer_init()
{
	  //CTC
    TCCR1B |= (1 << WGM12);
    //TOP
    OCR1A = 0x3D09;
    OCR1B = 0x30D;

    //prescaler, start
    TCCR1B |= (1 << CS12) | (1 << CS10);
    //interrupt on CompareA
    TIMSK1 |= (1 << OCIE1A) | (1 << OCIE1B);
    sei();


}

volatile int seconds = 0;
volatile char *str;
volatile int str_pointer, str_len;

int main() {

  uart_init();
  timer_init();
  //output
  LED_DDR |= _BV(LED);
  
  while (1) {
	set_sleep_mode(SLEEP_MODE_IDLE);
 	sleep_mode();
  }
}



ISR (TIMER1_COMPA_vect) 
{
  seconds++;
  

  char str2[20];
  sprintf(str2, "%d\n\r", seconds);
  str_len = strlen(str2);
  str_pointer = 0;
  str = str2;
  
  uart_transmit();
  LED_PORT |= _BV(LED);

}

ISR (TIMER1_COMPB_vect)
{
  LED_PORT &= ~_BV(LED);
}


// transmisja jednego znaku
int uart_transmit()
{

  if(str_pointer < str_len && (UCSR0A & _BV(UDRE0)))
  {
    UDR0 = str[str_pointer];
    str_pointer++;
  }

  return 0;
}

// przerwanie to jest wywoływane przez flage TXC w rejestrze UCSRA
//(gdy dane z Transmit shift register zostaną wysłane), która jest 
// resetowana automatycznie po obsłużeniu przerwania
ISR (USART_TX_vect)  
{
  uart_transmit();
}

