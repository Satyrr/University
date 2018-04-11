#include <avr/io.h>
#include <util/delay.h>

#define LED1 PB0
#define LED2 PB1
#define LED3 PB2
#define LED4 PB3
#define LED_DDR DDRB
#define LED_PORT PORTB
#define DELAY_TIME 100

int main() {
  LED_DDR |= _BV(LED1) | _BV(LED2) | _BV(LED3) | _BV(LED4);
  LED_PORT |= _BV(LED1);
  while (1) {
    LED_PORT |= _BV(LED2);
    _delay_ms(DELAY_TIME);
    
    for(int i = 0; i < 3; i++)
    {
      LED_PORT <<= 1;
      _delay_ms(DELAY_TIME);
    }
    
    for(int i = 0; i < 4; i++)
    {
      LED_PORT >>= 1;
      _delay_ms(DELAY_TIME);
    }
    
  }
}
