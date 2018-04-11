#include <stdio.h>
#include <stdlib.h>


void insert_sort(long *first, long *last);

int main()
{
    int n=0,i=0;
    /*
    long tab[10]={6,4,8,4,2,87,3,1,5,8};

    insert_sort(tab,tab+9);
    for(i=0;i<10;i++) printf("%ld, ",tab[i]);
    printf("\n");
    return 0;
    */
    long *tab2;
    
    printf("Podaj rozmiar tablicy:");
    scanf("%d",&n);
    tab2=malloc(n*sizeof(long));
    printf("Podaj %d liczb:",n);
    for(i=0;i<n;i++) scanf("%ld",tab2+i);
    insert_sort(tab2,tab2+n-1);
    for(i=0;i<n;i++) printf("%ld, ",tab2[i]);
    printf("\n");

    return 0;
   
}
