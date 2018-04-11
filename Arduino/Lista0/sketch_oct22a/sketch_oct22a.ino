#define LED_DDR DDRB
#define LED_PORT PORTB

volatile unsigned int clocks = 0;

int main() {
  // Output czujnika IR do PD2 
  PORTD = 0b00000100;
  // Dioda IR podpieta do PB0, Dioda LED do PB1
  DDRB |= 0b00000011;
  
  // Set the Timer Mode to CTC
  TCCR0A |= (1 << WGM01);
  // Set the value that you want to count to
  OCR0A = 0x07;
  TIMSK0 |= (1 << OCIE0A);    //Set the ISR COMPA vect
  sei();         //enable interrupts
  TCCR0B |= (1 << CS01) | (1 << CS00);
  // set prescaler to 128 and start the timer

  while (1) {
    // jezeli wykryto output z odbiornika, zaswiec diode LED
    if( !( PIND & _BV(PD2) ) )
      PORTB |= 0b00000010;
    else
      PORTB &= ~(0b00000010);
  }
}

// Tutaj migamy dioda IR
// Radze przebudowac system przerw w miganiu dioda IR
// czyli te ify z clocksami
// oraz poeksperymentowac z wartosciami licznika, bo duzo
// zalezy od odleglosci diody od odbiornika itd.
ISR (TIMER0_COMPA_vect)  // timer0 overflow interrupt
{
  if (clocks < 5000)
  {    
    PORTB ^= 0b00000001;
  }
  if (clocks > 10000)
    clocks = 0;
  clocks++;
}

