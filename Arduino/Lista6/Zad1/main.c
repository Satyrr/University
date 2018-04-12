#include <avr/io.h>
#include <stdio.h>
#include <inttypes.h>
#include <string.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <util/delay.h>

/* uart */

#define BAUD 9600                          // baudrate
#define UBRR_VALUE ((F_CPU)/16/(BAUD)-1)   // zgodnie ze wzorem

// inicjalizacja UART
void uart_init()
{
  // ustaw baudrate
  UBRR0 = UBRR_VALUE;
  // włącz odbiornik i nadajnik
  UCSR0B = _BV(RXEN0) | _BV(TXEN0);
  // ustaw format 8n1
  UCSR0C = _BV(UCSZ00) | _BV(UCSZ01);
}


// transmisja jednego znaku
int uart_transmit(char data, FILE *stream)
{
  // czekaj aż transmiter gotowy
  while(!(UCSR0A & _BV(UDRE0)));
  UDR0 = data;
  return 0;
}

// odczyt jednego znaku
int uart_receive(FILE *stream)
{
  // czekaj aż znak dostępny
  while (!(UCSR0A & _BV(RXC0)));
  return UDR0;
}

void print_uart(char signs[])
{
	int signs_len = strlen(signs);
	for(int i = 0; i < signs_len; i++)
	{
		uart_transmit(signs[i], NULL);
	}
}
/* End of uart */


void ADC_init()
{
	// Aref = Avcc, PIN = A0
	ADMUX |= (1 << REFS0);
	// enable adac, prescaler = 128, auto-trigger, run 
	ADCSRA = (1<<ADEN)| (1<<ADPS2)|(1<<ADPS1)|(1<<ADPS0) |(1<<ADATE)|(1<<ADSC);

}

void timer1_init()
{
  // ustaw tryb licznika 1
  // COM1B = 10   -- non-inverting mode
  // WGM1  = 1110 -- fast PWM top=ICR1
  // CS1   = 001  -- prescaler 0
  // ICR1  = 10000
  ICR1 = 1023;
  TCCR1A = _BV(COM1B1) ;
  TCCR1B =   _BV(WGM13) |  _BV(CS10);
}

#define LED1 PB2

#define LED_DDR DDRB
#define LED_PORT PORTB

#define ITERS 35
#define DELAY_TIME 5

int main() {
	uart_init();
	ADC_init();
	timer1_init();

  // ustaw pin OC1A (PB1) jako wyjście
  LED_DDR |= (1 << LED1);

  
  while (1) {

  	uint16_t adc = ADC;

    char str[80];
    sprintf(str, "adc: %d\n\r", adc);
    print_uart(str);

    OCR1B = adc;
    
    //_delay_ms(500);	
  	}
}