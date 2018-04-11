#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED_DDR DDRB
#define LED_PORT PORTB

#define IR_INPUT PD2
#define IR_PIN PIND
#define IR_PORT PORTD

volatile unsigned int ticks = 0;

int main() {
  LED_DDR |= 0b00000101;

  IR_PORT = 0b00000100;

  // Set the Timer Mode to CTC
  TCCR0A |= (1 << WGM01);

  // Set the value that you want to count to
  OCR0A = 0x50;

  TIMSK0 |= (1 << OCIE0A);    //Set the ISR COMPA vect

  sei();         //enable interrupts

  TCCR0B |= (1 << CS01);
  // set prescaler to 64 and start the timer

  while (1) {
    if( !( IR_PIN & _BV(IR_INPUT) ) )
    {
      LED_PORT |= 0b00000100;
      _delay_ms(200);
      LED_PORT &= ~0b00000100;
    }
      
  }
}

ISR (TIMER0_COMPA_vect)  // timer0 overflow interrupt
{
  ticks++;
  if (ticks < 5000)
  {
    /*
    PORTB |= 0b00000001;
    _delay_us(1);
    PORTB &= ~(0b00000001);
    _delay_us(25);
    */

    PORTB ^= 0b00000001;
  }
  else if (ticks > 10000)
    ticks = 0;
}

