Pracownia P2 

Prosty system plików.

Celem tego projektu jest utworzenie prostego systemu plików na wirtualnym dysku. W tym celu zaimplementuję podstawowe funkcje systemu plików (read, open, write..), które będzie można użyć w programie. Dane plików i meta-informacje systemu plików bedą znajdowały na wirtualnym dysku. Wirtualnym dyskiem będzie pojedynczy plik umieszczony w prawdziwym systemie plików.

Do implementacji dysku użyję pomocniczych funkcji znajdujących się w pliku disk.h/disk.c. Wirtualny dysk posiada 8,192 bloki, a każdy blok ma 4KB. Funkcje pozwalają na utworzenie, otwarcie lub zamknięcie dysku, odczyt lub zapis do bloków.

Pełna specyfikacja projektu : http://www.cs.ucsb.edu/~chris/teaching/cs170/projects/proj5.html
