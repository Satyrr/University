#include <stdio.h>
#include <stdlib.h>


unsigned long fibonacci(unsigned long n);

int main()
{
    unsigned long n=0;
    
    printf("Podaj nr wyrazu ciagu F:");
    scanf("%ld",&n);
    printf("%ld\n",fibonacci(n));

    return 0;
   
}
