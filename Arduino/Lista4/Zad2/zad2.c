#include <avr/io.h>
#include <util/delay.h>
#include <avr/interrupt.h>
#include <avr/sleep.h>

#define LED PB1
#define LED_DDR DDRB
#define LED_PORT PORTB

#define BTN PD7
#define BTN_PIN PIND
#define BTN_PORT PORTD

#define DELAY_TIME 1000


void timer_init()
{
	//CTC
    TCCR0A |= (1 << WGM01);
    //TOP
    OCR0A = 0x4E; // 15625 / 78 = 200
    //prescaler, start
    TCCR0B |= (1 << CS02) | (1 << CS00); //f = 16000000/1024 = 15625
    //interrupt on CompareA
    TIMSK0 |= (1 << OCIE0A);
    sei();


}
volatile char tab[200];
volatile int idx = 0;

int main() {

  timer_init();
  //pull-up input
  BTN_PORT |= _BV(BTN);
  //output
  LED_DDR |= _BV(LED);

  for(int i =0; i < 200; i++)
    tab[i] = 0;
  
  while (1) {
	set_sleep_mode(SLEEP_MODE_IDLE);
 	sleep_mode();
  }
}


ISR (TIMER0_COMPA_vect)  // timer0 overflow interrupt
{
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

}