#include <stdio.h>

extern int clz(long);

int main()
{
    long n=0;
    printf("Podaj liczbe:");
    scanf("%ld",&n);
    printf("Ilosc zer w prefiksie::%d\n",clz(n));
    return 0;
}
