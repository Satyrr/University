#include <stdio.h>
#include "filesystem.h"
#include <time.h>

//funkcja pomocnicza wypelniajaca tablice losowymi liczbami 
void random_tab(unsigned char *tab, int bytes)
{
	for(int i=0;i<bytes;i++)
		tab[i] = rand()%255;
}

int main()
{	

	printf("1. Utworz nowy system plikow\n2. Otworz istniejacy system plikow\nWybierz opcje:");
	int opcja=0;
	scanf("%d",&opcja);
	if(opcja==1)
		make_fs("dysk1");
	else if(opcja!=2)
	{
		printf("bledna opcja");
		return -1;
	}

	mount_fs("dysk1");
	ls();
	//utworzenie plikow
	fs_create("plik1");
	fs_create("plik2");

	// otwarcie plikow
	int fildes = fs_open("plik1");
	int fildes2 = fs_open("plik2");

	//tablice z danymi do zapisu, tab2 nadpisze dane z tab[4000:6000]
	unsigned char tab[6000];
	unsigned char tab2[2000];
	unsigned char tab3[6000];

	//wypelnienie tablic do zapisu
	random_tab(tab,6000);
	random_tab(tab2,2000);
	random_tab(tab3,6000);

	//tablice do odczytu
	unsigned char readtab[6000];
	unsigned char readtab2[6000];

	//zapisanie danych do plikow
	fs_write(fildes,(void*)tab,6000);
	fs_write(fildes2,(void*)tab3,6000);

	//obciecie plik1 do 4000 bajtow
	fs_truncate(fildes,4000);

	//zapisanie tab2 do plik1 
	fs_write(fildes,(void*)(tab2),2000);

	//ustawienie seek_p na poczatek pliku
	fs_lseek(fildes,0);
	fs_lseek(fildes2,0);

	//odczyt danych 
	fs_read(fildes,(void*)readtab,6000);
	fs_read(fildes2,(void*)readtab2,6000);

	//porownanie zapisanych i odczytanych danych
	
	for(int i=0;i<4000;i++)
		printf("tab[%d] = %d, readtab[%d] = %d\n",i,tab[i],i,readtab[i]);
	for(int i=0;i<2000;i++)
		printf("tab2[%d] = %d, readtab[%d] = %d\n",i,tab2[i],i+4000,readtab[i+4000]);

	for(int i=0;i<6000;i++)
		printf("tab3[%d] = %d, readtab2[%d] = %d\n",i,tab3[i],i,readtab2[i]);
	
	

	fs_close(fildes);
	fs_close(fildes2);

	ls();

	umount_fs("dysk1");

	return 0;
}
