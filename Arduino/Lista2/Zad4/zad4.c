#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>

#define LEN_OF_NOTE(note) (note & 0x0f)
#define PIT_OF_NOTE(note) ((note & 0xf0) >> 4)
static inline void delay_ms(uint16_t count);

/*
NUTA : 
4 pierwsze bity - wysokosc
4 ostatnie bity - dlugosc
Wysokosci nut:
 0 : pauza
 1 : C
 2 : D
 3 : E
 4 : F
 5 : G
 6 : A
 7 : B
 8 : C
Dlugosci nut:
 0 : cala nuta - 3 sekundy
 1 : 1/2 nuty 
 2 : 1/4 nuty 
 3 : 1/8 nuty 
 4 : 1/16 nuty 
*/

int notes[] = 
{
  0x23,
  0x24,
  0x32,
  0x22,
  0x52,
  0x41,

  0x23,
  0x24,
  0x32,
  0x22,
  0x62,
  0x51,

  0x23,
  0x24,
  0x82,
  0x72,
  0x52,
  0x42,
  0x32,
  0x83, 
  0x84,
  0x72,
  0x52,
  0x62,
  0x51
};


void timer1_init()
{
  // ustaw tryb licznika
  // COM1A = 10   -- non-inverting mode
  // WGM1  = 1110 -- fast PWM top=ICR1
  // CS1   = 101  -- prescaler 1024
  // częstotliwość 16e6/(64+(1+ICR1))
  // wzór: datasheet 20.12.3 str. 164
  TCCR1A = _BV(COM1A1) | _BV(WGM11);
  TCCR1B = _BV(WGM12) | _BV(WGM13) | _BV(CS10) | _BV(CS11);
  ICR1 = 0xff;
  // ustaw pin OC1A (PB1) jako wyjście
  DDRB |= _BV(PB1);
}

int main()
{
  int melody_len = sizeof(notes)/sizeof(notes[0]);

  int note_lengths[] = 
  {
    3000, //cala
    1500, //1/2
    750, //1/4
    375, //1/8
    185 //1/16
  };

  int ICRI_notes[] = 
  {
    12,  //  pauza
    957,  // C
    850,  // D
    760,  // E
    716,  // F
    637,  // G
    568,  // a
    507,  // b
    478   // c
  };

  // uruchom licznik
  timer1_init();
  // ustaw wypełnienie 50%
  OCR1A = ICR1/2;
  
      
  while(1)
  {
    for(int i = 0; i < melody_len; i++)
    {
      uint8_t note_len = LEN_OF_NOTE(notes[i]);
      uint8_t note_pit = PIT_OF_NOTE(notes[i]);

      ICR1 = ICRI_notes[note_pit];
      OCR1A = ICR1 >> 1;

      delay_ms(note_lengths[note_len]);
      OCR1A = 0;
      //_delay_ms(50);
    }
    OCR1A = 0;
    _delay_ms(1000);
  }
}


static inline void delay_ms(uint16_t count) {
  while(count--) {
    _delay_ms(1);

  }
}