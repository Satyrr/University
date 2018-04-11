#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LED PB1
#define LED_DDR DDRB
#define LED_PORT PORTB

#define BTN PD7
#define BTN_PIN PIND
#define BTN_PORT PORTD

#define DELAY_TIME 1000


int main() {
  BTN_PORT |= _BV(BTN);
  LED_DDR |= _BV(LED);

  char tab[200];
  for(int i =0; i < 200; i++)
    tab[i] = 0;
  int idx = 0;
  while (1) {

    if( !(BTN_PIN & _BV(BTN)))
    {
      tab[idx] = 1;
    }
    
    idx = (idx + 1) % 200;

    if(tab[idx % 200] == 1)
    {
      LED_PORT |= _BV(LED);
      tab[idx % 200] = 0;
    }
    else
      LED_PORT &= ~_BV(LED);

    _delay_ms(5);
    
  }
}
