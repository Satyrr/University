/*
  POV demo by Patrick Roanhouse
*/

#include <avr/io.h>
#include <util/delay.h>
#define DELAYTIME 3                                              /* ms */

uint8_t Cool[] = {
  0b00011111,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00011111,
  0b00000000,
  0b00000000,
  0b00000000,
  0b00011111,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00011111,
  0b00000000,
  0b00000000,
  0b00000000,
  0b00011111,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00010001,
  0b00011111,
  0b00000000,
  0b00000000,
  0b00000000,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b00000001,
  0b11111111,
};


int main(void) {
  uint8_t i;

  DDRB = 0xff;                                      

  while (1) {                                            
    for (i = 0; i < sizeof(Cool); i++) {
           
      PORTB = Cool[i];
      _delay_ms(DELAYTIME);
    }
    PORTB = 0;                    
    _delay_ms(5 * DELAYTIME);
  }                                                    
  return 0;
}
