#include <stdio.h>
#include <string.h>
#include <pthread.h>
#include <stdlib.h>
#include <unistd.h>
#include <semaphore.h>

typedef struct _Buffer {
	/*
	*struktura reprezentujaca pojedynczy bufor
	*/
	int 	*buffer;
	int 	buffer_id;
	int 	consume_pos, produce_pos;
	int 	consumed, produced;
	sem_t 	access, to_consume, empty_slots;
	/* semafory odpowiedzalne za:
	*access - dostep do bufora
	*to_consume - reprezentuje ilosc danych na buforze
	*empty_slots - ilosc wolnych miejsc w buforze
	*/

} Buffer;

Buffer* 			_Buffer; //tablica buforow
	
static int 			_Buffer_size=10000;
static int 			_Buffer_count=100;
static int 			_Consumers_per_buffer=10;
static int 			_Producers_per_buffer=10;
static int 			_Production_limit=300; //maksymalna ilosc produktow na jeden bufor

//semafor odpowiadajacy za blokowanie konsumentow, ktorzy maja wiecej danych od innych
static sem_t* 		_Consumers_sems; 


static int 			_Consumer_id=0, _Producer_id=0;
//semafory wykorzystywane do przyjdzielania id konsumentom i producentom
static sem_t 		_Consumer_id_sem; 
static sem_t 		_Producer_id_sem;


static int 			_Consumers_left; //liczba konsumentow, ktorzy maja mniej danych od innych
static sem_t 		_Consumer_left_sem; //semafor uzywany do uaktualniania wartosci_Consumers_left


static pthread_t*	_Threads; //tablica watkow

static sem_t 		_Print_sem;


void alloc_buffers(Buffer *buffer)
{
	for(int i=0;i<_Buffer_count;i++)
	{
		buffer[i].buffer_id=i;

		buffer[i].consume_pos=0;
		buffer[i].produce_pos=0;
		buffer[i].consumed=0;
		buffer[i].produced=0;

		buffer[i].buffer=malloc(_Buffer_size*sizeof(int));

		sem_init(&buffer[i].access,0,1);
		sem_init(&buffer[i].to_consume,0,0);
		sem_init(&buffer[i].empty_slots,0,_Buffer_size);
	}
}

void initialize_consumers_sems()
{
	for(int i=0;i<_Buffer_count*_Consumers_per_buffer;i++)
	{
		sem_init(&_Consumers_sems[i],0,0);
	}
}

void *producer(void *buffer)
{
	Buffer *b=(Buffer*) buffer;
	int id;
	/*
	*przydzielanie id producentom jest ograniczone przez semafor
	*aby dwoch producentow nie dostalo tego samego id
	*/
	sem_wait(&_Producer_id_sem);
	id=_Producer_id;
	_Producer_id++;
	sem_post(&_Producer_id_sem);

	while(b->produced<_Production_limit)
	{
		int product = rand()%200+1; //produkcja

		sem_wait(&b->empty_slots);//sprawdzenie czy niepelny
		sem_wait(&b->access);//zajecie bufora

		if(b->produced==_Production_limit)
		{
			sem_post(&b->empty_slots);
			sem_post(&b->access); 
			return NULL;
		}
		b->buffer[b->produce_pos]=product;
		b->produce_pos=(b->produce_pos+1)%_Buffer_size;
		b->produced++;
		

		sem_wait(&_Print_sem);
		printf("Bufor #%d: Producent o id:%d wyprodukowal produkt:%d\n",
				b->buffer_id,id,product);
		printf("Wyprodukowanych:%d\n\n",b->produced);
		sem_post(&_Print_sem);
		
		sem_post(&b->access); //zwolnienie bufora
		sem_post(&b->to_consume);//zwiekszenie ilosci danych,
		
	}

	return NULL;
}

void reset_consumer_sems()
{
	for(int i=0;i<_Consumers_per_buffer*_Buffer_count;i++)
		sem_post(&_Consumers_sems[i]);
}

void *consumer(void *buffer)
{
	Buffer *b = (Buffer*) buffer;
	int id;
	int consumer_consumed=0;

	/*
	*przydzielanie id konsumentom jest ograniczone przez semafor
	*aby dwoch konsumentow nie dostalo tego samego id
	*/
	sem_wait(&_Consumer_id_sem);
	id=_Consumer_id;
	_Consumer_id++;
	sem_post(&_Consumer_id_sem);

	while(b->consumed<_Production_limit)
	{
		sem_wait(&b->to_consume);//sprawdzenie czy bufor pusty
		sem_wait(&b->access);//zajecie bufora

		if(b->consumed==_Production_limit)
		{
			sem_post(&b->to_consume);
			sem_post(&b->access);
			return NULL;
		}

		int product=b->buffer[b->consume_pos];
		b->buffer[b->consume_pos]=0;
		b->consume_pos=(b->consume_pos+1)%_Buffer_size;
		b->consumed++; consumer_consumed++;

		sem_wait(&_Print_sem);
		printf("Bufor#%d: Konsument%d zabral produkt:%d\n",
				b->buffer_id,id,product);
		printf("ilosc produktow skonsumowanych przez Konsument%d :%d\n\n",id,consumer_consumed);
		sem_post(&_Print_sem);

		sem_post(&b->access);//zwolnij buffer
		sem_post(&b->empty_slots);//zwieksz ilosc wolnych miejsc
		
		/*
		*_Consumers_left oznacza ilosc konsumentow, ktorzy nie otrzymali jeszcze
		*danych w danej kolejce. Ostatni konsument, ktory otrzymuje dane, resetuje
		*semafory pozostalych konsumentow czekajacych na swoja kolej. Konsumenci
		*zmniejszaja _Consumers_left pomiedzy semaforami aby uniknac sytuacji, w
		*ktorej dwoch konsumentow resetuje pozostalych.
		*/
		sem_wait(&_Consumer_left_sem);
		_Consumers_left--;
		if(_Consumers_left==0)
		{
			reset_consumer_sems();
			_Consumers_left=_Buffer_count*_Consumers_per_buffer;
		}
		sem_post(&_Consumer_left_sem);

		//oczekiwanie na konsumentow, ktorzy maja mniej danych
		sem_wait(&_Consumers_sems[id]);
	}
	return NULL;
}

void free_sems()
{
	for(int i=0;i<_Buffer_count*_Consumers_per_buffer;i++)
	{
		sem_destroy(&_Consumers_sems[i]);
	}
	free(_Consumers_sems);

	sem_destroy(&_Consumer_id_sem);
	sem_destroy(&_Producer_id_sem);
	sem_destroy(&_Consumer_left_sem);
	sem_destroy(&_Print_sem);
}

int main()
{
	_Consumers_left=_Buffer_count*_Consumers_per_buffer;

	_Buffer=malloc(_Buffer_count*sizeof(Buffer));
	_Consumers_sems=malloc(_Buffer_count*_Consumers_per_buffer*sizeof(sem_t));

	alloc_buffers(_Buffer);

	sem_init(&_Consumer_id_sem,0,1);
	sem_init(&_Producer_id_sem,0,1);
	sem_init(&_Consumer_left_sem,0,1);
	initialize_consumers_sems();
	sem_init(&_Print_sem,0,1);

	
	
	int thread_count=_Buffer_count*(_Consumers_per_buffer+_Producers_per_buffer);
	_Threads=malloc(thread_count*sizeof(pthread_t));
	int error;
	for(int i=0;i<_Buffer_count;i++)
	{
		int offset=(_Consumers_per_buffer+_Producers_per_buffer)*i;
		for(int k=offset;k<_Producers_per_buffer+offset;k++)
		{
			error = pthread_create(&(_Threads[k]), NULL, &producer, (void *) &_Buffer[i]);
        	if (error != 0)
            	printf("\nNie można utworzyć wątku : %s", strerror(error));
		}
		offset=offset+_Producers_per_buffer;
		for(int k=offset;k<_Consumers_per_buffer+offset;k++)
		{
			error = pthread_create(&(_Threads[k]), NULL, &consumer, (void *) &_Buffer[i]);
	        if (error != 0)
	            printf("\nNie można utworzyć wątku : %s", strerror(error));
		}
        
	}


    for(int i=0;i<thread_count;i++)
    	pthread_join(_Threads[i],NULL);

    for(int i=0;i<_Buffer_count;i++)
    	free(_Buffer[i].buffer);
    free(_Buffer);
    free(_Threads);
    free_sems();

}