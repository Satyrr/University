// program to change brightness of an LED
// demonstration of PWM
  
#include <avr/io.h>
#include <util/delay.h>
#include <math.h>
  
#define BREATHES_PER_MINUTE 15
#define BRIGHT_CHANGE_FREQ 100
#define PI 3.14159265

struct RGB
{
    int R; // OC0A
    int G; // OC2A
    int B; // OC2B
};

struct RGB get_rgb()
{
    // Kolor HSV przy pełnym nasyceniu ma postać, w której
    // jedna współrzędna jest równa 0, druga równa 255,
    // a trzecia w zakresie <0,255> ( http://colorizer.org/ )

    uint8_t full_color = TCNT0 % 3;
    uint8_t zero_color = ( full_color + 1 + (TCNT0 % 2) ) % 3;
    uint8_t var_color;
    if(full_color + 1 % 3 == zero_color)
        var_color = full_color + 2 % 3;
    else
        var_color = full_color + 1 % 3;

    uint8_t colors[3];
    colors[full_color] = 255;
    colors[zero_color] = 0;
    colors[var_color] = TCNT2;

    struct RGB color;
    color.R = colors[0]; color.G = colors[1]; color.B = colors[2];

    return color;
}

void timer0_init()
{
    // ustaw tryb licznika
    // COM0A = 11   -- inverting mode
    // WGM0  = 0011 -- fast PWM top=0xff
    // CS1   = 001  -- prescaler 8
    // częstotliwość 16e6/8 = 2 MHz
    TCCR0A |= (1<<WGM00)|(1<<COM0A0)|(1<<COM0A1)|(1<<WGM01);
    TCCR0B |= (1 << CS01);
}

void timer2_init()
{
    // ustaw tryb licznika
    // COM2A,COM2B = 11   -- inverting mode
    // WGM1  = 0011 -- fast PWM, top = 0xff
    // CS2   = 001  -- prescaler 8
    // częstotliwość 16e6/8 = 2 MHz
    TCCR2A |= (1<<WGM20)|(1<<COM2B1)|(1<<COM2B0)|(1<<COM2A0)|(1<<COM2A1)|(1<<WGM21);
    TCCR2B |= (1 << CS21);
    
}

void generate_sin_values(int sin_values[])
{
    for(int i=0; i < BRIGHT_CHANGE_FREQ; i++)
    {
        float ifloat = (float)i;
        sin_values[i] = pow(4096,sin((PI/2.0f)*(ifloat/BRIGHT_CHANGE_FREQ))) ;
    }
}
  
int main()
{
    timer0_init();
    timer2_init();
    DDRB |= (1<<PB3);
    DDRD |= (1<<PD3) | (1<<PD6);

    int sin_values[BRIGHT_CHANGE_FREQ];
    generate_sin_values(sin_values);

    struct RGB color;
    
    while(1)
    {
        color = get_rgb();
        
        for(int i=0; i<BRIGHT_CHANGE_FREQ; i++)
        {
            OCR0A = (color.R * (long)sin_values[i]) >> 12;
            OCR2A = (color.G * (long)sin_values[i]) >> 12;
            OCR2B = (color.B * (long)sin_values[i]) >> 12;
            _delay_ms(18);
        }
        
        for(int i=BRIGHT_CHANGE_FREQ-1; i>=0; i--)
        {
            OCR0A = (color.R * (long)sin_values[i]) >> 12;
            OCR2A = (color.G * (long)sin_values[i]) >> 12;
            OCR2B = (color.B * (long)sin_values[i]) >> 12;
            _delay_ms(18);
        }

        _delay_ms(200);

    }
    
}