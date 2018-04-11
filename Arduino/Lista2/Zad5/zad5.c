#include <avr/io.h>
#include <stdio.h>
#include <inttypes.h>
#include <math.h>
#include <string.h>
#include <avr/interrupt.h>
#include <avr/pgmspace.h>

#define BAUD 9600                          // baudrate
#define UBRR_VALUE ((F_CPU)/16/(BAUD)-1)   // zgodnie ze wzorem

#define ADD_IDX 0
#define MUL_IDX 1
#define DIV_IDX 2
#define MOD_IDX 3
#define POW_IDX 2

#define MAX_TICKS 0xffff
#define ITERCNT 500

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

//uzywany do uzyskania odpowiedniego wypelnienia fali
void timer1_init()
{
  // ustaw tryb licznika 1
  // WGM1  = 0000 -- Normal
  // CS1   = 001  -- prescaler 0
  TCCR1B = _BV(CS10);
}

unsigned short compute_ticks(unsigned short start, unsigned short end)
{
	if(end > start)
		return end - start;
	else //overflow
		return (MAX_TICKS - start) + end;
}

void int8measure(unsigned long ticks_sums[])
{
	uint8_t num1 = 5;
	uint8_t num2 = 3;

	for(int i = 0; i < 4; i++)
		ticks_sums[i] = 0;
	
	unsigned short start, end;
	//dodawanie
	start = TCNT1;
	num1 += num2;
	end = TCNT1;
	ticks_sums[ADD_IDX] = compute_ticks(start, end);

	//mnozenie
	start = TCNT1;
	num2 *= num1;
	end = TCNT1;
	ticks_sums[MUL_IDX] = compute_ticks(start, end);

	//modulo
	start = TCNT1;
	num1 %= num2;
	end = TCNT1;
	ticks_sums[MOD_IDX] = compute_ticks(start, end);
	
	//dzielenie
	start = TCNT1;
	num1 /= num2;
	end = TCNT1;
	ticks_sums[DIV_IDX] = compute_ticks(start, end);
}

void int16measure(unsigned long ticks_sums[])
{
	uint16_t num1 = 2555;
	uint16_t num2 = 38548;

	for(int i = 0; i < 4; i++)
		ticks_sums[i] = 0;
	
	unsigned short start, end;
	//dodawanie
	start = TCNT1;
	num1 += num2;
	end = TCNT1;
	ticks_sums[ADD_IDX] = compute_ticks(start, end);

	//mnozenie
	start = TCNT1;
	num2 *= num1;
	end = TCNT1;
	ticks_sums[MUL_IDX] = compute_ticks(start, end);

	//modulo
	start = TCNT1;
	num1 %= num2;
	end = TCNT1;
	ticks_sums[MOD_IDX] = compute_ticks(start, end);
	
	//dzielenie
	start = TCNT1;
	num2 /= num1;
	end = TCNT1;
	ticks_sums[DIV_IDX] = compute_ticks(start, end);
}

void int32measure(unsigned long ticks_sums[])
{
	uint32_t num1 = 5152;
	uint32_t num2 = 34552;

	for(int i = 0; i < 4; i++)
		ticks_sums[i] = 0;
	
	unsigned short start, end;
	//dodawanie
	start = TCNT1;
	num1 += num2;
	end = TCNT1;
	ticks_sums[ADD_IDX] = compute_ticks(start, end);

	//mnozenie
	start = TCNT1;
	num2 *= num1;
	end = TCNT1;
	ticks_sums[MUL_IDX] = compute_ticks(start, end);

	//modulo
	start = TCNT1;
	num1 %= num2;
	end = TCNT1;
	ticks_sums[MOD_IDX] = compute_ticks(start, end);
	
	//dzielenie
	start = TCNT1;
	num2 /= num1;
	end = TCNT1;
	ticks_sums[DIV_IDX] = compute_ticks(start, end);
}

void floatmeasure(unsigned long ticks_sums[])
{
	float num1 = 5.0;
	float num2 = 3.0;

	for(int i = 0; i < 3; i++)
		ticks_sums[i] = 0;
	
	unsigned short start, end;
	//dodawanie
	start = TCNT1;
	num1 += num2;
	end = TCNT1;
		
	ticks_sums[ADD_IDX] = compute_ticks(start, end);
	

	//mnozenie
	start = TCNT1;
	num2 *= num1;
	end = TCNT1;
		
	ticks_sums[MUL_IDX] = compute_ticks(start, end);
	
	//potęgowanie
	start = TCNT1;
	num1 = powf(num1, num2);
	end = TCNT1;
	
	ticks_sums[POW_IDX] = compute_ticks(start, end);
}

void doublemeasure(unsigned long ticks_sums[])
{
	double num1 = 5.4;
	double num2 = 3.5;

	for(int i = 0; i < 3; i++)
		ticks_sums[i] = 0;
	
	unsigned short start, end;
	//dodawanie
	start = TCNT1;
	num1 += num2;
	end = TCNT1;
		
	ticks_sums[ADD_IDX] = compute_ticks(start, end);
	

	//mnozenie
	start = TCNT1;
	num2 *= num1;
	end = TCNT1;
		
	ticks_sums[MUL_IDX] = compute_ticks(start, end);
	
	//potęgowanie
	start = TCNT1;
	num1 = powf(num1, num2);
	end = TCNT1;
	
	ticks_sums[POW_IDX] = compute_ticks(start, end);
}

void print_int_measure(unsigned long intticks[], char* int_type)
{
	char str[100];
	print_uart("TYP : int");
	print_uart(int_type);
	print_uart("\n\r");

	sprintf(str, "%lu", intticks[ADD_IDX]);
	print_uart("Czas dodawania: ");
	print_uart(str);
	print_uart("\n\r");

	sprintf(str, "%lu", intticks[MUL_IDX]);
	print_uart("Czas mnozenia: ");
	print_uart(str);
	print_uart("\n\r");

	sprintf(str, "%lu", intticks[DIV_IDX]);
	print_uart("Czas dzielenia: ");
	print_uart(str);
	print_uart("\n\r");

	sprintf(str, "%lu", intticks[MOD_IDX]);
	print_uart("Czas modulo: ");
	print_uart(str);
	print_uart("\n\r");
	print_uart("\n\r");
	print_uart("\n\r");
}

void print_fd_measure(unsigned long fdticks[], char* type )
{
	char str[100];
	print_uart("TYP : ");
	print_uart(type);
	print_uart("\n\r");

	sprintf(str, "%lu", fdticks[ADD_IDX]);
	print_uart("Czas dodawania: ");
	print_uart(str);
	print_uart("\n\r");

	sprintf(str, "%lu", fdticks[MUL_IDX]);
	print_uart("Czas mnozenia: ");
	print_uart(str);
	print_uart("\n\r");

	sprintf(str, "%lu", fdticks[POW_IDX]);
	print_uart("Czas potegowania: ");
	print_uart(str);
	print_uart("\n\r");
	print_uart("\n\r");
	print_uart("\n\r");
}

int main()
{
  // zainicjalizuj UART
  uart_init();
  timer1_init();

  unsigned long int8ticks[4], int16ticks[4], int32ticks[4], floatticks[3], doubleticks[3];

  int8measure(int8ticks);
  int16measure(int16ticks);
  int32measure(int32ticks);

  floatmeasure(floatticks);
  doublemeasure(doubleticks);

  while(1) {
    int a = uart_receive(NULL);

    if(a == '1')
    {
    	print_int_measure(int8ticks, "8");
    	print_int_measure(int16ticks, "16");
    	print_int_measure(int32ticks, "32");

    	print_fd_measure(floatticks, "float");
    	print_fd_measure(doubleticks, "double");
    }
  }
}