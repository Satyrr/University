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


#define _SS PD7
#define _MOSI PD6
#define _SCK PD5


// SPI Slave Device
void spi_init()
{
    DDRB = _BV(PB4);   //MISO as OUTPUT
    SPCR = (1<<SPE) | (1<<DORD) | (1<<SPIE);   //Enable SPI, najmniejszy bit jako pierwszy, przerwania

    // Konfiguracja pinów mastera
    PORTD |= _BV(_SS);
    DDRD |= _BV(_SS) | _BV(_MOSI) | _BV(_SCK);

}

//wysyla pierwszy bit argumentu do spi slave i zwraca odebrany bit od slave
unsigned char spi_transmit(unsigned char bit)
{

  unsigned char res_bit = 0;

  // Select slave
  PORTD &= ~_BV(_SS);

  // Send bit
  if(bit&1)
  {
    PORTD |= _BV(_MOSI);
  }
  else
  {
    PORTD &= ~_BV(_MOSI);
  }

  // Clock 
  PORTD |= _BV(_SCK);

  // Recv bit
  if((PIND & _BV(PD4)))
  {
    res_bit = 1;
  }

  _delay_us(10);
  PORTD &= ~_BV(_SCK);

  return res_bit;
}

int main() {

  uart_init();
  spi_init();
  sei();

  int seconds = 0;
  char seconds_str[5];
  char recv_message[5];
  int recv_byte_nr = 0;
  while (1) {

    sprintf(seconds_str, "%d", seconds);
    print_uart("\n\n\rMaster wysyla: ");
    print_uart(seconds_str);


    for(int chars=0;chars<=strlen(seconds_str);chars++)
    {
      char recv_byte = 0; // odebrany bajt
      char c = seconds_str[chars]; // bajt do wyslania
      for(int bits=0;bits<8;bits++)
      {
        //odebranie/wyslanie 8 bitow
        char res = spi_transmit(c&1);
        recv_byte |= res<<bits;
        c >>=1;
      }

      recv_message[recv_byte_nr] = recv_byte;
      recv_byte_nr++;

      if(recv_byte == 0)
      {
        recv_byte_nr = 0;
        print_uart(", Master odebral: ");
        print_uart(recv_message);
      }
    }
    
    _delay_ms(1000);
    seconds++;
  }
}

ISR(SPI_STC_vect)
{
  unsigned char c = SPDR;
  SPDR = c;
}