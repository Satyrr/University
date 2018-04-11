#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED_DDR DDRB
#define LED_PORT PORTB

#define BTN_RESET PD4
#define BTN_PREV PD3
#define BTN_NEXT PD2
#define BTN_PIN PIND
#define BTN_PORT PORTD

#define DEBOUNCE_DELAY 50

volatile unsigned long miliseconds = 0;

int main() {
  BTN_PORT |= 0b00011100;
  LED_DDR |= 0b00011111;

  // Set the Timer Mode to CTC
  TCCR0A |= (1 << WGM01);

  // Set the value that you want to count to
  OCR0A = 0xF9;

  TIMSK0 |= (1 << OCIE0A);    //Set the ISR COMPA vect

  sei();         //enable interrupts

  TCCR0B |= (1 << CS01) | (1 << CS00);
  // set prescaler to 64 and start the timer
  
  char click_processed = 0;
  char number = 0;
  unsigned long lastDebounceTime = 0;
 
  while (1) {

    if(miliseconds - lastDebounceTime > DEBOUNCE_DELAY)
    {
      //jeżeli żaden przycisk nie jest wcisniety -
      //umożliw kolejne wciśnięcia
      if ( (BTN_PIN & _BV(BTN_RESET)) &&
           (BTN_PIN & _BV(BTN_NEXT)) &&
           (BTN_PIN & _BV(BTN_PREV)) &&
           click_processed == 1)
      {
        click_processed = 0;
        lastDebounceTime = miliseconds;
      }
      // jeżeli przycisk nie jest przytrzymywany
      else if( !(BTN_PIN & _BV(BTN_RESET)) && 
        click_processed != 1)
      {
        click_processed = 1;
        number = 0;
        lastDebounceTime = miliseconds;
      }
      else if( !(BTN_PIN & _BV(BTN_PREV)) && 
        click_processed != 1)
      {
        click_processed = 1;
        number--;
        if(number == -1)
          number = 31;
        lastDebounceTime = miliseconds;
      }
      else if( !(BTN_PIN & _BV(BTN_NEXT)) && 
        click_processed != 1)
      {
        click_processed = 1;
        number++;
        if(number == 32)
          number = 0;
        lastDebounceTime = miliseconds;
      }
    }
    //generate gray code
    LED_PORT = number ^ (number >> 1);
  }
}

ISR (TIMER0_COMPA_vect)  // timer0 overflow interrupt
{
    miliseconds++;
}
