#include <avr/io.h>
#include <stdio.h>
#include <string.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include <stdlib.h>
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
  // enable adac, prescaler = 128, interrupt enable 
  ADCSRA = (1<<ADEN)| (1<<ADPS2)|(1<<ADPS1)|(1<<ADPS0)|(1<<ADIE);
}

volatile int32_t UAverage;
volatile int32_t averages[200];
volatile uint16_t iters;

int32_t variance()
{
    int32_t result = 0;

    for(int i = 0; i < 200; i++)
    {
      int32_t diff = (int32_t)averages[i] - (int32_t)UAverage;
      result += diff*diff;
    }

    return result/200;
}

int main() {
  uart_init();
  ADC_init();
  while (1) {

    cli();
    //pomiar
    UAverage = 0;
    for(int i=0; i<200;i++)
    {
      ADCSRA |= (1<<ADSC);
      while(ADCSRA & (1<<ADSC));

      averages[i] = ((int32_t)1023*1100)/((int32_t)ADC);
      UAverage += averages[i];
    }
    UAverage = UAverage/200; 
    int32_t vari = variance();

    
    char str[80];
    sprintf(str, "Napiecie Vcc: %limV, wariancja: %li\n\r\n\ra", UAverage, vari);
    print_uart(str);

    UAverage = 0;
    iters = 0;
    set_sleep_mode(SLEEP_MODE_ADC);
    while(iters < 200)
    {
      sei();
      
      sleep_mode();
      cli();

    }
    UAverage = UAverage/200; 
    vari = variance();

    sprintf(str, "***(Noise Canceler)*** Napiecie Vcc: %limV, wariancja: %li\n\r\n\rb", UAverage, vari);
    print_uart(str);

    _delay_ms(2000);
  }
}

ISR(ADC_vect)
{
  averages[iters] = ((int32_t)1023*1100)/((int32_t)ADC);
  UAverage += averages[iters];
  iters++;
  ADCSRA |= (1<<ADSC); 

}