#include <avr/io.h>
#include <util/delay.h>

#define LED1 PB0
#define LED2 PB1

#define LED_DDR DDRB
#define LED_PORT PORTB
#define DELAY_TIME 100

int main() {
  LED_DDR |= 0x1f;
  LED_PORT |= _BV(LED1);
  while (1) {
    LED_PORT |= _BV(LED2);
    _delay_ms(DELAY_TIME);
    
    for(int i = 0; i < 4; i++)
    {
      LED_PORT <<= 1;
      _delay_ms(DELAY_TIME);
    }
    
    for(int i = 0; i < 5; i++)
    {
      LED_PORT >>= 1;
      _delay_ms(DELAY_TIME);
    }
    
  }
}