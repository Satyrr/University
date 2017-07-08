#ifndef FILESYSTEM_H
#define FILESYSTEM_H

#include <stddef.h>
#include "disk.h"
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <string.h>


int make_fs(char *disk_name);
int mount_fs(char *disk_name);
int umount_fs(char *disk_name);

int fs_open(char *name);
int fs_close(int fildes);
int fs_create(char *name);
int fs_delete(char *name);

int fs_read(int fildes, void *buf, size_t nbyte);
int fs_write(int fildes, void *buf, size_t nbyte);
int fs_get_filesize(int fildes);
int fs_lseek(int fildes, int offset);
int fs_truncate(int fildes, int length);


int init_meta();
int free_tables();
int free_fildes();

int load_meta();
int save_meta();

int filename_exists(char *name);
int file_opened(char *name);
int preceding_blocks(int seek_p);
int find_free_block();

void ls();

/* Zmienna mowiaca czy system plikow jest zamontowany: 0 - nie, 1 - tak*/
int mounted;

/* Porcja pliku na dysku*/
typedef struct Fs_File_portion
{
	short start, length;
} fs_file_portion;

/* Wpis w tablicy plikow */
typedef struct Fs_File_entry
{
	char name[16];
	int size;
	fs_file_portion* portions;
	short portion_count;
	
} fs_file_entry;

/* Tablica FAT */
typedef struct Fs_File_allocation_table
{
	char file_count;
	fs_file_entry *entry[64];
} fs_file_allocation_table;

/* Tablica alokacji blokow na dysku */
typedef struct Fs_Disc_allocation_table
{
	char blocks[4096];
} fs_disc_allocation_table;

/* File descriptor */
typedef struct Fs_fildes
{
	int seek_p;
	int file_entry_number;
	char name[16];
} fs_fil_des;

fs_file_allocation_table fs_fat;
fs_disc_allocation_table fs_dat;
fs_fil_des *fs_fildes[32];
int fildes_active;

#endif