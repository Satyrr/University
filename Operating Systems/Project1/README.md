# SOP1


Problem synchronizacji procesów - Producent - Konsument

Jest to klasyczny problem synchronizacji procesów, w którym dwa procesy: jeden - producent, drugi - konsument, działają równocześnie i dzielą wspólny bufor. Producent umieszcza w buforze nowe dane, a konsument je 'konsumuje'. 

Pojawiają sie trzy problemy:
1. producent nie może umieszczać danych na pełnym buforze
2. konsument nie może zdejmować danych z pustego bufora
3. producent i konsument nie mogą korzystać z bufora w tym samym czasie

W mojej rozszerzonej wersji problemu występują dwa bufory, dwóch producentów oraz dwóch konsument. Konsumenci muszą być zsynchronizowani, tzn. jeżeli jeden z nich ma więcej danych niż drugi, to musi na niego zaczekać.

Taki problem może mieć uzasadnienie np. przy oglądaniu filmów online. Komputer odbiera dwa rodzaje danych: dzwięk i obraz, następnie odtwarza je, jednak nie może tego robić asynchronicznie.

***UPDATE***

Program dodatkowo rozszerzyłem o możliwość utworzenia dowolnej ilości buforów oraz producentów/konsumentów przypadających na jeden bufor. Poza tym można ustalić rozmiar bufora oraz limit produkcji poprzez modyfikacje odpowiednich zmiennych globalnych.
