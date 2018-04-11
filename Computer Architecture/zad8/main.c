#include <stdio.h>
#include <stdlib.h>
double approx_sqrt(double x, double epsilon);

int main()
{
    printf("%f\n",approx_sqrt(2,0.0001));
    return 0;
}
