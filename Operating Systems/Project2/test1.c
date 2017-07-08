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
	srand(time(NULL));
	make_fs("dysk1");
	mount_fs("dysk1");

	//utworzenie plikow
	fs_create("plik1");
	fs_create("plik2");
	fs_create("plik3");
	fs_create("plik4");

	// otwarcie plikow
	int fildes = fs_open("plik1");
	int fildes2 = fs_open("plik2");

	//tablice z danymi do zapisu
	unsigned char tab[6000];

	//wypelnienie tablic do zapisu
	random_tab(tab,6000);


	//zapisanie danych do plikow
	for(int i=0;i<1000;i++)
	{
		fs_write(fildes,(void*)tab,400);
		fs_write(fildes2,(void*)tab,400);		
	}

	fs_close(fildes);
	fs_close(fildes2);

	ls();

	umount_fs("dysk1");

	return 0;
}