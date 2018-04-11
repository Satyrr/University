#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED_DDR DDRB
#define LED_PORT PORTB
#define LED1 PB4

#define IR_LED PB0
#define IR_INPUT PD2
#define IR_PIN PIND
#define IR_PORT PORTD

// Do przerywania swiecenia dioda IR
volatile unsigned int ticks = 0;

int main() {
  LED_DDR |= _BV(LED1) | _BV(IR_LED);

  IR_PORT = _BV(IR_INPUT);

  // Prescaler timera ustawiony na 8
  // TOP ustawiony na 52
  // czest. : 16e6/(8*(52+1)) = 37735 Hz
  TCCR0A |= (1 << WGM01);
  TCCR0B |= (1 << CS01);
  OCR0A = 0x1f;
  TIMSK0 |= (1 << OCIE0A);
  sei();

  while (1) {
    if( !( IR_PIN & _BV(IR_INPUT) ) )
    	LED_PORT |= _BV(LED1);
    else 
    	LED_PORT &= ~_BV(LED1);
  }
}

ISR (TIMER0_COMPA_vect)
{
  if (ticks < 10000)
  {
    //modulacja swiecenia dioda
    LED_PORT ^= _BV(IR_LED);
  }
  else if (ticks > 30000)
    ticks = 0;
  ticks++;
}
