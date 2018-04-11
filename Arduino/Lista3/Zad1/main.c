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
  ICR1 = 10000;
  TCCR1A = _BV(COM1B1) | _BV(WGM11);
  TCCR1B = _BV(WGM12) | _BV(WGM13) | _BV(CS10);
}

#define LED1 PB2

#define LED_DDR DDRB
#define LED_PORT PORTB

#define ITERS 35
#define DELAY_TIME 5

uint32_t get_fotoresistor_resistance(int adcAverage)
{
	float fotoresistor_U = 5.0*(adcAverage/1023.0);
	uint32_t result = (uint32_t)( (fotoresistor_U*10000.0)/(5.0-fotoresistor_U) );

    if(result > 90000) return 90000; //maximum resistance
    if(result < 30) return 30; //minimum resistance

	return result;
}

int main() {
	uart_init();
	ADC_init();
	timer1_init();

	// ustaw pin OC1A (PB1) jako wyjście
  LED_DDR |= (1 << LED1);
  //LED_PORT |= _BV(LED1);

  uint16_t adcAverage;
  while (1) {

  	adcAverage = 0;
  	for(int i=0; i<ITERS;i++)
  	{
  		adcAverage += ADC;
  		_delay_ms(DELAY_TIME);
  	}
  	adcAverage = adcAverage/ITERS;

  	uint32_t fotoresistor_R = get_fotoresistor_resistance(adcAverage);
  	OCR1B = (10000*(fotoresistor_R-30))/89970; // min_res = 30, max_res = 90000 (ohm)

  	//TO DEBUG
    char str[80];
    sprintf(str, "fotoresistor resistance: %"PRIu32"Ohm, adc: %d\n\r", get_fotoresistor_resistance(adcAverage), adcAverage);
    print_uart(str);
    	
  	}
}