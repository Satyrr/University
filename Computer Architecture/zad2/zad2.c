#include <stdio.h>

typedef struct {
    unsigned long lcm, gcd;
} result_t;

result_t lcm_gcd(unsigned long, unsigned long);
int main()
{
    unsigned long n1, n2=0;
    printf("Podaj liczby :");
    scanf("%ld %ld",&n1, &n2);
    result_t t=lcm_gcd(n1,n2);
    printf("NWW: %ld NWD: %ld\n",t.lcm,t.gcd);
    return 0;
}
