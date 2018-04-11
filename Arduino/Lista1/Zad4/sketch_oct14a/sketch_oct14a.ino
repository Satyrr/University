#include <avr/io.h>
#include <util/delay.h>

#define SHORT_SIG_TIME 300
#define BETWEEN_SIG_TIME 300
#define LONG_SIG_TIME 900
#define BETWEEN_LETTER_TIME 900

#define BUZZER PB5

int incomingByte = 0;   // for incoming serial data

// short signal - 1, long signal - 2,
unsigned char letters[] = 
{
  0x09, //A 0000 1001
  0x56, //B 0101 0110
  0x66, //C 0110 0110
  0x16, //D 0001 0110
  0x01, //E 0000 0001
  0x65, //F 0110 0101
  0x19, //G 0001 1010
  0x55, //H 0101 0101
  0x05, //I 0000 0101
  0xA9, //J 1010 1001
  0x26, //K 0010 0110
  0x59, //L 0101 1001
  0x09, //M 0000 1010
  0x05, //N 0000 0110
  0x2A, //O 0010 1010
  0x69, //P 0110 1001
  0x9A, //Q 1001 1010
  0x19, //R 0001 1001
  0x15, //S 0001 0101
  0x02, //T 0000 0010
  0x25, //U 0010 0101
  0x95, //V 1001 0101
  0x29, //W 0010 1001
  0x96, //X 1001 0110
  0xA6, //Y 1010 0110
  0x5A //Z 0101 1010
};

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);

  DDRB = 0xff;
  
}

void loop() {
          if (Serial.available() > 0) {
                // read the incoming byte:
                incomingByte = Serial.read();

                Morse(incomingByte);
                _delay_ms(BETWEEN_LETTER_TIME);
        }

}

void Morse(int letter)
{
  int idx = letter - 'A';
  if(idx < 0 || idx > 25) return;

  unsigned char code = letters[idx];

  while(code > 0)
  {
    unsigned char sig = code & 0x03;
    code >>= 2;
    
    //zapal
    PORTB |= 0x01;
    PORTB |= _BV(BUZZER);
    
    if(sig & 2)
      _delay_ms(LONG_SIG_TIME);
    else
      _delay_ms(SHORT_SIG_TIME);
      
    //zgaś
    PORTB &= ~0x01;
    PORTB &= ~_BV(BUZZER);
    //odstęp pomiędzy sygnałem w elemencie
    _delay_ms(BETWEEN_SIG_TIME);
  }
  
}
