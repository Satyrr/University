#include <avr/io.h>
#include <stdio.h>
#include <inttypes.h>
#include <string.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <util/delay.h>
#include <math.h>
#include <stdlib.h>


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
  // Aref = Avcc, PIN = Internal 1.1V
  ADMUX |= (1 << REFS0) | (1 << MUX3)| (1 << MUX2)| (1 << MUX1);
  // enable adac, prescaler = 128, auto-trigger, run 
  ADCSRA = (1<<ADEN)| (1<<ADPS2)|(1<<ADPS1)|(1<<ADPS0) |(1<<ADATE)|(1<<ADSC);
}

void timer1_init()
{
  // ustaw tryb licznika 1
  // COM1A = 10   -- non-inverting mode
  // WGM1  = 1110 -- fast PWM top=ICR1
  // CS1   = 001  -- prescaler 0
  // ICR1  = 1023
  ICR1 = 1023;
  TCCR1A = _BV(COM1A1) | _BV(WGM11);
  TCCR1B = _BV(WGM12) | _BV(WGM13) | _BV(CS10);
}


int main() {
  uart_init();
  ADC_init();
  timer1_init();
  DDRB = 0x02;
  OCR1A = 1000;
  uint8_t heater_on = 1;
  uint16_t adcAverage;
  while (1) {

    //pomiar
    adcAverage = 0;
    for(int i=0; i<60;i++)
    {
      adcAverage += ADC;
     _delay_ms(17);
    }
    adcAverage = adcAverage/60; 
    /*
      adcAverage/1023 = 1.1V / AVcc => 
      AVcc = 1023*1.1V/adcAverage
    */
    uint32_t Avcc = ((uint32_t)1023*1100)/((uint32_t)adcAverage);
    
    char str[80];
    if(heater_on == 1)
    {
      sprintf(str, "Grzalka ON, Napiecie Vcc: %dmV\n\r", Avcc);
      heater_on = 0;
      OCR1A = 20;
    }
    else
    {
      sprintf(str, "Grzalka OFF, Napiecie Vcc: %dmV\n\r", Avcc);
      heater_on = 1;
      OCR1A = 1000;
    }
    print_uart(str);
  }
}