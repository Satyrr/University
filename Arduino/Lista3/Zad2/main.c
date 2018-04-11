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

#define RESISTOR_R 2200.0
#define DELAY_TIME 100

void ADC_init()
{
  // Aref = Avcc, PIN = A0
  ADMUX |= (1 << REFS0);
  // enable adac, prescaler = 128, auto-trigger, run 
  ADCSRA = (1<<ADEN)| (1<<ADPS2)|(1<<ADPS1)|(1<<ADPS0) |(1<<ADATE)|(1<<ADSC);

}

float get_termistor_resistance(int averageVal)
{
  float U1 = 5.0*(1.0-averageVal/1023.0); // resistor U
  float U2 = 5.0-U1; // termistor U

  float I_total = U1/RESISTOR_R; //current
  return U2/I_total;

}

float get_temperature_in_C(int averageVal)
{
  float termistor_resistance = get_termistor_resistance(averageVal);
  float res = log(termistor_resistance/3000.68); // R/R0
  res = res/(5000.0); // beta
  res = res + 1.0/294.0; // 1/T0
  return (1.0/res) - 273.15;
}

int main() {
     uart_init();
     ADC_init();

    uint16_t adcAverage;
    while (1) {

      adcAverage = 0;
      for(int i=0; i<60;i++)
      {
        adcAverage += ADC;
        _delay_ms(17);
      }
      adcAverage = adcAverage/60;

      float temp = get_temperature_in_C(adcAverage);
      int temp_int_x10 = (int)(temp*10);
      char str[50];
      sprintf(str, "%d.%d stopni C, %d\n\r",temp_int_x10/10,temp_int_x10%10,adcAverage);
      print_uart(str);
    }
}


//745 - 20.6 C/19.4 C
//782 - 20.2 C/17.8 C
//755 - ../18.5
//742 - ../19.5
//740 - ../19.7
//738 - ../20.0
//736 - ../20.2
//735 - ../20.5
//734 - ../20.5
//733 - ../20.8
//731 - ../21.2

//Beta = 3124.0