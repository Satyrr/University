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

int main(void)
{
uart_init();
    DDRB = 0x07;                      // Setup PB0, PB1 and PB2 as output

    
    ADMUX |= (1<<REFS0)|(1<<MUX2);    // Set Reference to AVCC and input to ADC4
    ADCSRA |= (1<<ADEN)|(1<<ADPS2)    // Enable ADC, set prescaler to 16
             |(1<<ADIE);              // Fadc=Fcpu/prescaler=1000000/16=62.5kHz
                                      // Fadc should be between 50kHz and 200kHz
                                      // Enable ADC conversion complete interrupt

    sei();                            // Set the I-bit in SREG

    ADCSRA |= (1<<ADSC);              // Start the first conversion
    PORTB |= 0x04;                    // Indicate the start of the conversion
  
  
    for(;;)                           // Endless loop;
    {
        PORTB^= 0x02;                 // Toggle PB1

    }                                 // main() will never be left

    return 0;                         // This line will never be executed

}


// Interrupt subroutine for ADC conversion complete interrupt
ISR(ADC_vect) 
{
        PORTB &= ~0x04;               // Indicate the end of the conversion

        if(ADC >= 512)                // Compare the conversionresult with 512
            PORTB |= 0x01;            // If larger, set PB0
        else
            PORTB &= ~0x01;           // If smaller, reset PB0

        ADCSRA |= (1<<ADSC);          // Start the next conversion
        PORTB |= 0x04;                // Indicate the start of the conversion
        char str[80];
    sprintf(str, "Napiecie Vcc: ");
    print_uart(str);
}