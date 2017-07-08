#include "filesystem.h"

/* Zainicjalozwanie pustych tablic */
int init_meta()
{
	mounted = 0;

	free_tables();
	free_fildes();

	return 0;
}

int free_tables()
{
	fs_fat.file_count=0;

	for(int i=0;i<64;i++)
	{
		fs_file_entry* entry = fs_fat.entry[i];
		if(entry!=NULL)
		{
			free(entry->portions);
			free(fs_fat.entry[i]);
			fs_fat.entry[i] = NULL;
		}
	}

	for(int i=0;i<4096;i++)
		fs_dat.blocks[i]=0;

	return 0;
}

int free_fildes()
{
	for(int i=0;i<32;i++)
	{
		if(fs_fildes[i]!=NULL) free(fs_fildes[i]->name);
		free(fs_fildes[i]);
	}
	return 0;
}

/* Utworzenie systemu plików, poprzez utworzenie dysku, otwarcie go i zainicjalizowanie metadanych.
Meta dane pustego dysku to same zera, dlatego przy tworzeniu systemu plikow nie trzeba nic zapisywac.
Jeżeli operacja sie nie udala - zwraca -1 */
int make_fs(char *disk_name)
{

	if(make_disk(disk_name) < 0) 
		return -1;

	if(open_disk(disk_name) < 0) 
		return -1;

	init_meta();

	if(close_disk(disk_name) < 0) 
		return -1;

	return 0;
}

/* Zaladowanie meta informacji z dysku 
Jeżeli operacja sie nie udala - zwraca -1 */
int load_meta()
{	
	/* Blok nr 0 : Odczyt tablicy alokacji dysku */
	if(block_read(0,fs_dat.blocks) < 0) 
		return -1;

	/* Blok nr 1 : Odczyt FAT */
	char* table = malloc(4096*sizeof(char));

	if(block_read(1, table) < 0)
		return -1;

	/* Odczyt kolejnych wpisow tablicy FAT 
		Struktura FAT :
		Bajt 0 - liczba plikow w systemie
		k - numer pliku*22 { k = 0,1,2,...,file_count }
		Bajty od 1+k do 16+k - nazwa pliku
		Bajty 17+k do 20+k - rozmiar pliku 
		Bajty 21+k do 22+k - ilosc porcji
	*/
	fs_fat.file_count = table[0];

	for(int i=0;i<fs_fat.file_count;i++)
	{
		fs_fat.entry[i] = malloc(sizeof(fs_file_entry));
		fs_fat.entry[i]->size = 0;
		fs_fat.entry[i]->portion_count = 0;

		/* Odczyt nazwy pliku */
		for(int j=0;j<16;j++)
			fs_fat.entry[i]->name[j] = table[22*i+1+j];

		/* Odczyt rozmiaru pliku */
		for(int j=0;j<4;j++)
			fs_fat.entry[i]->size += ((unsigned char)table[22*i+17+j]) << (24-j*8);

		/* Odczyt liczby porcji */
		for(int j=0;j<2;j++)
			fs_fat.entry[i]->portion_count += (table[22*i+21+j]) << (8-j*8);

		/*  Bloki zawierajace tablice porcji danego pliku */
		char* index_block = malloc(4096*sizeof(char));
		if(block_read(2+i, index_block) < 0)
			return -1;

		fs_fat.entry[i]->portions = malloc(fs_fat.entry[i]->portion_count*sizeof(fs_file_portion));

		/* Odczyt porcji danego pliku */
		for(int j=0;j<fs_fat.entry[i]->portion_count;j++)
		{
			fs_fat.entry[i]->portions[j].start = 0;
			fs_fat.entry[i]->portions[j].start += index_block[j*4] << 8;
			fs_fat.entry[i]->portions[j].start += index_block[j*4+1]; 

			fs_fat.entry[i]->portions[j].length = 0;
			fs_fat.entry[i]->portions[j].length += index_block[j*4+2] << 8;
			fs_fat.entry[i]->portions[j].length += index_block[j*4+3]; 
		}

		free(index_block);
	}

	free(table);
	return 0;
}


/* Zamontowanie systemu plikow 
Jeżeli operacja sie nie udala - zwraca -1 */
int mount_fs(char *disk_name)
{

	if(open_disk(disk_name)<0)
		return -1;

	if(load_meta()<0)
		return -1;

	mounted = 1;

	return 0;
}

/* Zapisanie meta informacji na dysku 
Jeżeli operacja sie nie udala - zwraca -1 */
int save_meta()
{
	/* Blok nr 0 : Zapis tablicy alokacji dysku */
	if(block_write(0,fs_dat.blocks) < 0) 
		return -1;

	char* table = malloc(4096*sizeof(char));

	table[0] = fs_fat.file_count;

	/* Zapis kolejnych wpisow tablicy FAT 
		Struktura FAT :
		Bajt 0 - liczba plikow w systemie
		k - numer pliku*22 { k = 0,1,2,...,file_count }
		Bajty od 1+k do 16+k - nazwa pliku
		Bajty 17+k do 20+k - rozmiar pliku 
		Bajty 21+k do 22+k - ilosc porcji
	*/

	/* Zmienna sluzaca do zapisu plikow w jednym nieprzewanym ciagu */
	int files_saved = 0;

	for(int i=0;i<64;i++)
	{
		/* Opuszczenie pustych wierszy tablicy */
		if(fs_fat.entry[i]==NULL) continue;

		/* Zapis nazwy pliku */
		for(int j=0;j<16;j++)
			table[22*files_saved+1+j] = fs_fat.entry[i]->name[j];

		/* Zapis rozmiaru pliku */
		for(int j=0;j<4;j++)
			table[22*files_saved+17+j] = (fs_fat.entry[i]->size >> (3-j)*8) & 255;	

		/* Zapis liczby porcji pliku */
		for(int j=0;j<2;j++)
			table[22*files_saved+21+j] = (fs_fat.entry[i]->portion_count >> (1-j)*8) & 255; 

		char* index_block = malloc(4096*sizeof(char));
		
		/* Zapis porcji danego pliku */
		for(int j=0;j<fs_fat.entry[i]->portion_count;j++)
		{
			index_block[j*4] = ((fs_fat.entry[i]->portions[j].start >> 8) & 255);
			index_block[j*4+1] = (fs_fat.entry[i]->portions[j].start & 255);

			index_block[j*4+2] = ((fs_fat.entry[i]->portions[j].length >> 8) & 255);
			index_block[j*4+3] = (fs_fat.entry[i]->portions[j].length & 255); 
		}

		if(block_write(2+files_saved, index_block) < 0)
			return -1;

		free(index_block);
		files_saved++;
	}

	/* Blok nr 1 : Zapis FAT */
	if(block_write(1, table) < 0)
		return -1;

	free(table);
	return 0;
}

/* Odmnontowanie dysku 
Jeżeli operacja sie nie udala - zwraca -1 */
int umount_fs(char *disk_name)
{
	if(save_meta()<0)
		return -1;

	if(close_disk()<0)
		return -1;

	mounted = 0;
	return 0;
}

/* Otwarcie pliku o nazwie name 
Jeżeli operacja sie nie udala - zwraca -1 */
int fs_open(char *name)
{
	if(mounted==0)
	{
		printf("Can't open file: filesystem not mounted\n");
		return -1;
	}

	if(fildes_active>32)
	{
		printf("Can't open file: Can't open more files.\n");
		return -1;
	}

	/* Fildes to int umozliwiajacy operacje na otwartym pliku(odczyt i zapis) */
	int fildes=-1;

	/* Sprawdzenie czy plik o nazwie name istnieje */
	int entry_number = filename_exists(name);
	if(entry_number == -1) return -1;

	for(int i=0;i<32;i++)
	{
		/* Przyznanie wolnego fildes'a */
		if(fs_fildes[i]==NULL)
		{
			fs_fildes[i] = malloc(sizeof(fs_fil_des));

			fs_fildes[i]->seek_p = 0;
			fs_fildes[i]->file_entry_number = entry_number;
			strcpy(fs_fildes[i]->name,name);

			fildes = i;
			fildes_active++;

			break;
		}
	}
	
	return fildes;
}

/* Zamkniecie pliku 
Jeżeli operacja sie nie udala - zwraca -1 */
int fs_close(int fildes)
{
	if(mounted==0)
	{
		printf("Can't close file: filesystem not mounted\n");
		return -1;
	}

	if(fildes>31)
	{
		printf("Invalid fildes number");
		return -1;
	}

	if(fs_fildes[fildes]!=NULL)
	{
		/* Usuniecie istniejacego fildesa */
		free(fs_fildes[fildes]);
		fs_fildes[fildes]=NULL;
		fildes_active--;
		return 0;
	}

	return -1;
}

/* Funkcja sprawdzajaca czy plik o nazwie name istnieje, zwraca jego indeks w tablicy FAT 
Jeżeli operacja sie nie udala - zwraca -1 */
int filename_exists(char *name)
{
	for(int i=0;i<64;i++)
	{
		if(fs_fat.entry[i]!=NULL)
		{
			if(strcmp(fs_fat.entry[i]->name,name)==0) return i;
		}
	}
	return -1;
}

/* Stworzenie pliku o nazwie name 
Jeżeli operacja sie nie udala - zwraca -1 */
int fs_create(char *name)
{
	if(mounted==0)
	{
		printf("Can't create file: filesystem not mounted\n");
		return -1;
	}

	if(strlen(name)>15)
	{
		printf("Can't create file: Name of the file is too long.\n");
		return -1;
	}

	if(fs_fat.file_count>=64)
	{
		printf("Can't create file: Too many files.\n");
		return -1;
	}

	if(filename_exists(name)>-1)
	{
		printf("Can't create file %s: Name of the file already exists.\n",name);
		return -1;
	}


	for(int i=0;i<64;i++)
	{
		/* Zapisanie pliku do wolnego miejsca w FAT */
		if(fs_fat.entry[i]==NULL)
		{
			fs_fat.entry[i]=malloc(sizeof(fs_file_entry));
			strcpy(fs_fat.entry[i]->name,name);
			fs_fat.entry[i]->size = 0;
			fs_fat.entry[i]->portions = NULL;
			fs_fat.entry[i]->portion_count = 0;

			fs_fat.file_count++;

			return 0;
		}
	}

	return -1;

}

/* Sprawdzenie czy plik jest otwarty */
int file_opened(char *name)
{
	for(int i=0;i<32;i++)
	{
		if(fs_fildes[i]!=NULL)
		{
			if(strcmp(fs_fildes[i]->name,name)==0)
				return 1;
		}
	}

	return 0;

}

/* Usuniecie pliku z systemu plikow 
Jeżeli operacja sie nie udala - zwraca -1 */
int fs_delete(char *name)
{
	if(mounted==0)
	{
		printf("Can't delete file: filesystem not mounted\n");
		return -1;
	}

	if(filename_exists(name)<0)
	{
		printf("Can't delete file: File %s doesn't exist.\n",name);
		return -1;
	}

	if(file_opened(name))
	{
		printf("Can't delete file: File %s is opened. Close it before deleting.\n",name);
		return -1;
	}
	
	/* Indeks pliku w FAT */
	int file_number = filename_exists(name);

	fs_file_entry *file = fs_fat.entry[file_number];

	/* Zwolnienie blokow na dysku */
	for(int i=0;i<file->portion_count;i++)
	{
		for(int j=file->portions[i].start ; j<file->portions[i].length ; j++)
		{
			fs_dat.blocks[j-4096]=0;
		}
	}

	/* Usuniecie pliku z FAT i zmniejszenie ilosci plikow  */
	free(fs_fat.entry[file_number]);
	fs_fat.entry[file_number]=NULL;
	fs_fat.file_count--;

	return 0;


}

/* Nr bloku w ktorym znajduje sie aktualnie seek_p */
int preceding_blocks(int seek_p)
{
	int block_nr = 0;
	while(4096*(block_nr+1) <= seek_p) block_nr++;
	return block_nr;
}

/* Odczytanie nbyte bajtow pliku do bufora *buf z pliku do ktorego przypisany jest fildes 
Jeżeli operacja sie nie udala - zwraca -1 */
int fs_read(int fildes, void *buf, size_t nbyte)
{
	
	if(mounted==0)
	{
		printf("Can't read from file: filesystem not mounted\n");
		return -1;
	}

	if(fs_fildes[fildes]==NULL)
	{
		printf("Can't read from file: File isn't opened\n");
		return -1;
	}

	if(nbyte<1) return 0;

	/* Ilosc odczytanych bajtow */
	int readen_bytes=0;

	/* indeks pliku w tabeli FAT oraz seek_pointer */
	int file_index = fs_fildes[fildes]->file_entry_number;
	fs_file_entry* file = fs_fat.entry[file_index];
	int seek_p = fs_fildes[fildes]->seek_p;

	/* Jezeli seek_p znajduje sie na koncu pliku */
	if(file->size <= seek_p) return 0;

	/* Ilosc blokow przed seek_p */
	int blocks = preceding_blocks(seek_p);
	
	/* Nr bloku oraz nr porcji, w ktorych znajduje sie seek_p */
	int block_nr = file->portions[0].start, portion_index = 0;
	while(blocks > 0)
	{
		if(blocks >= file->portions[portion_index].length)
		{
			blocks -= file->portions[portion_index].length;
			portion_index++;
			block_nr = file->portions[portion_index].start;
		}
		else
		{
			block_nr = file->portions[portion_index].start + blocks;
			blocks = 0;
		}
	}

	char blockbuf[4096];
	block_read(block_nr,blockbuf);

	while(seek_p < file->size && readen_bytes < nbyte)
	{
		/* offset seek_p w bloku, w ktorym sie znajduje */
		int offset = seek_p % 4096;

		/* Zapisanie bajtu do wynikowej tablicy */
		((char*)buf)[readen_bytes] = blockbuf[offset];

		readen_bytes++;
		seek_p++;

		if(seek_p > file->size) break;

		/* Sprawdzenie czy koniec bloku */
		if(offset == 4095)
		{
			/* Sprawdzenie czy koniec porcji  */
			if(block_nr + 1 > file->portions[portion_index].start + file->portions[portion_index].length - 1)
			{
				portion_index++;
				block_nr = file->portions[portion_index].start;

			}
			else
				block_nr++;
			/* Wczytanie kolejnego bloku */
			block_read(block_nr,blockbuf);
		}
	}

	/* Update seek_p */
	fs_fildes[fildes]->seek_p = seek_p;

	return readen_bytes;
}

/* Pomocnicza funkcja zwracajaca pierwszy wolny blok na dysku, lub -1 jesli dysk jest pelny */
int allocate_free_block()
{
	for(int i=0;i<4096;i++)
	{
		if(fs_dat.blocks[i]==0)
		{
			fs_dat.blocks[i]=1;
			return i+4096;
		}
	}

	return -1;
}

/* Zapisanie nbyte bajtow z bufora buf do pliku, do ktorego odwoluje sie fildes 
Jeżeli operacja sie nie udala - zwraca -1 */
int fs_write(int fildes, void *buf, size_t nbyte)
{
	if(mounted==0)
	{
		printf("Can't read from file: filesystem not mounted\n");
		return -1;
	}

	if(fs_fildes[fildes]==NULL)
	{
		printf("Can't write to file: File isn't opened\n");
		return -1;
	}

	if(nbyte < 1) return 0;

	/* Ilosc zapisanych bajtow */
	int written_bytes=0;

	/* indeks pliku w tabeli FAT oraz seek_pointer */
	int file_index = fs_fildes[fildes]->file_entry_number;
	fs_file_entry* file = fs_fat.entry[file_index];
	int seek_p = fs_fildes[fildes]->seek_p;

	if(seek_p > file->size)
	{
		printf("Error: seek_p bigger than file size");
		return -1;
	}

	/* Zaalokowanie pierwszego bloku */
	if(file->size == 0)
	{

		int start = allocate_free_block();
		if(start == -1)
		{
			printf("Can't write to file: disk run out of space ");
			return -1;
		}

		file->portions = malloc(sizeof(fs_file_portion));
		file->portion_count = 1;
		
		file->portions[0].start = start;
		file->portions[0].length = 1;
	}

	/* Zaalokowanie nowego bloku jesli seek_p jest na koncu pliku i bloku*/
	if(file->size == seek_p && seek_p != 0 && seek_p % 4096 == 0)
	{
			int last_block = file->portions[file->portion_count-1].start + file->portions[file->portion_count-1].length - 1;
			/* Sprawdzenie czy kolejny blok jest wolny */
			if(fs_dat.blocks[last_block - 4095]==0)
			{
				file->portions[file->portion_count-1].length++;
				fs_dat.blocks[last_block - 4095] = 1;
			}
			/*  Nowa porcja */
			else
			{
				int block_nr = allocate_free_block();
				if(block_nr == -1)
				{
					printf("Can't write to file: disk run out of space ");
					return -1;
				}

				file->portion_count++;
				file->portions = realloc(file->portions,sizeof(fs_file_portion)*file->portion_count);

				/*  Poczatek porcji */
				file->portions[file->portion_count-1].start = block_nr;
				file->portions[file->portion_count-1].length = 1;
			}
	}

	/* Ilosc blokow przed seek_p */
	int blocks = preceding_blocks(seek_p);
	
	/* Nr bloku oraz nr porcji, w ktorych znajduje sie seek_p */
	int block_nr = file->portions[0].start, portion_index = 0;
	while(blocks > 0)
	{
		if(blocks >= file->portions[portion_index].length)
		{
			blocks -= file->portions[portion_index].length;
			portion_index++;
			block_nr = file->portions[portion_index].start;
		}
		else
		{
			block_nr = file->portions[portion_index].start + blocks;
			blocks = 0;
		}
	}

	/* Zapis do pliku */
	char blockbuf[4096];
	block_read(block_nr,blockbuf);
	while(written_bytes < nbyte)
	{
		/* offset seek_p w bloku, w ktorym sie znajduje */
		int offset = seek_p % 4096;

		/* Zapisanie bajtu do wynikowej tablicy */
		blockbuf[offset] = ((char*)buf)[written_bytes];

		written_bytes++;
		seek_p++;

		if(written_bytes == nbyte) break;

		/* Sprawdzenie czy koniec bloku */
		if(offset == 4095)
		{
			/* Zapisanie wyczerpanego bloku */
			block_write(block_nr,blockbuf);

			/* Sprawdzenie czy kolejny blok pliku znajduje sie w aktualnej porcji */
			if(block_nr + 1 < file->portions[portion_index].start + file->portions[portion_index].length)
			{
				block_nr++;
			}
			/* Sprawdzenie czy istnieje kolejna porcja pliku */
			else if(portion_index+1 < file->portion_count)
			{
				portion_index++;
				block_nr = file->portions[portion_index].start;
			}
			/* Sprawdzenie czy kolejny blok jest wolny */
			else if(fs_dat.blocks[block_nr-4096+1]==0)
			{
				block_nr++;
				fs_dat.blocks[block_nr-4096] = 1;
				file->portions[portion_index].length++;
			}
			/*  Nowa porcja */
			else
			{
				block_nr = allocate_free_block();
				if(block_nr == -1)
				{
					printf("Can't write to file: disk run out of space ");
					/* Upadte file size*/
					file->size = fs_fildes[fildes]->seek_p + written_bytes;

					/* Update seek_p */
					fs_fildes[fildes]->seek_p = seek_p;
					return -1;
				}
	
				portion_index++;

				file->portion_count++;
				file->portions = realloc(file->portions, sizeof(fs_file_portion)*file->portion_count);

				/*  Poczatek nowej porcji */
				file->portions[portion_index].start = block_nr;
				file->portions[portion_index].length = 1;
			}

			block_read(block_nr,blockbuf);
		}
	}

	/* Update file size */
	if(file->size < fs_fildes[fildes]->seek_p + written_bytes) file->size = fs_fildes[fildes]->seek_p + written_bytes;

	/* Update seek_p */
	fs_fildes[fildes]->seek_p = seek_p;

	/* Zapisanie zmian na dysku */
	block_write(block_nr,blockbuf);

	return written_bytes;
}

/*  Zwraca rozmiar pliku, do ktorego odwoluje sie fildes
Jeżeli operacja sie nie udala - zwraca -1 */
int fs_get_filesize(int fildes)
{
	if(mounted==0)
	{
		printf("Can't get filesize: filesystem not mounted\n");
		return -1;
	}

	if(fs_fildes[fildes]==NULL)
	{
		printf("Can't get filesize: File isn't opened\n");
		return -1;
	}

	int file_index = fs_fildes[fildes]->file_entry_number;
	return fs_fat.entry[file_index]->size;
}

/* Ustawia aktualny seek pointer file descriptora na wartosci offset*/
int fs_lseek(int fildes, int offset)
{
	if(mounted==0)
	{
		printf("Can't lseek file: filesystem not mounted\n");
		return -1;
	}

	if(offset<0)
	{
		printf("Can't lseek file: Offset can't be less than 0.");
		return -1;
	}

	if(fs_fildes[fildes]==NULL)
	{
		printf("Can't lseek file: File isn't opened\n");
		return -1;
	}

	if(offset>fs_get_filesize(fildes))
	{
		printf("Can't lseek file: File size less than offset\n");
		return -1;
	}
	/*  Ustawienie nowego seek_p */
	fs_fildes[fildes]->seek_p = offset;

	return 0;
}

/*  Obciecie pliku do length pierwszych bajtow */
int fs_truncate(int fildes, int length)
{
	if(mounted==0)
	{
		printf("Can't truncate file: filesystem not mounted\n");
		return -1;
	}

	if(length<0)
	{
		printf("Can't truncate file: Length can't be less than 0.");
		return -1;
	}

	if(fs_fildes[fildes]==NULL)
	{
		printf("Can't truncate file: File not opened\n");
		return -1;
	}

	fs_fil_des* descriptor = fs_fildes[fildes];
	int entry_number = fs_fildes[fildes]->file_entry_number;
	fs_file_entry* file = fs_fat.entry[entry_number];

	/*  Rozszerzanie pliku niemozliwe */
	if(length > file->size)
	{
		printf("Can't truncate file: File's size less than length.'\n");
		return -1;
	}

	/*  Jezeli seek_p znajduje sie dalej niz rozmiar przycietego pliku, nalezy go ustawic na koniec nowego pliku */
	if(length < descriptor->seek_p) descriptor->seek_p = length;
	
	/*  Numer ostatniej porcji */
	int portion_index = file->portion_count-1;
	/*  Numer ostatniego bloku pliku */
	int block_nr = file->portions[portion_index].start + file->portions[portion_index].length - 1;
	
	while(file->size > length)
	{
		int bytes_in_last_block = file->size % 4096;
		if(bytes_in_last_block==0) bytes_in_last_block = 4096;

		/*  Dealokowanie kolejnych bajtow/blokow pliku */
		if(file->size - bytes_in_last_block >= length)
		{
			fs_dat.blocks[block_nr-4096] = 0;

			file->size -= bytes_in_last_block;
			file->portions[portion_index].length--;

			if(file->portions[portion_index].length==0)
			{
				portion_index--;
				block_nr = file->portions[portion_index].start + file->portions[portion_index].length - 1;
				file->portions = realloc(file->portions,file->portion_count-1);
				file->portion_count--;
			}
			else
				block_nr--;
		}
		else
			/*  Ustawienie odpowiedniego rozmiaru pliku*/
			file->size = length;
	}

	return 0;
}

//funkcja pomocnicza wypisujaca pliki w systemie
void ls()
{
	for(int i=0;i<64;i++)
	{
		if(fs_fat.entry[i]!=NULL)
		{
			printf("\n***\n");
			printf("(%d) Filename: %s, size: %d, ",i,fs_fat.entry[i]->name,fs_fat.entry[i]->size);
			printf("Ilosc porcji zajetych przez plik: %d\n",fs_fat.entry[i]->portion_count);
			if(fs_fat.entry[i]->portions!=NULL)
			{
				for(int j=0;j<fs_fat.entry[i]->portion_count;j++)
					printf("porcja [%d] start=%d dlugosc=%d \n",j,fs_fat.entry[i]->portions[j].start,fs_fat.entry[i]->portions[j].length);
			}
		
		}

	}
}