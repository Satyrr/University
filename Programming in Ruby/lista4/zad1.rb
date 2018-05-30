module Wypozyczalnia
	#Modul moze byc wmixowany w klase, ktorej obiekty posiadaja nastepujace tablice: przedmioty(zawiera przedmioty, ktore mozna aktualnie wypozyczyc),
	# wypozyczone(zawiera obiekty reprezentujace wypozyczenia) oraz klienci(zawiera obiekty reprezentujace klientow). Kazdy obiekt przedmiotu powinien zawierac akcesor idPrzedmiotu, 
	# kazdy obiekt wypozyczenia powinien zawierac akcesory idPrzedmiotu, przedmiot oraz idKlienta, kazdy obiekt klienta powinien zawierac akcesor idKlienta.
	@@id_przedmiotow=0
	@@id_klientow=0

	def wypozycz(idPrzedmiotu, idKlienta)
		przedmiot = self.przedmioty.find {|x| x.idPrzedmiotu=idPrzedmiotu } # wyszukanie przedmiotu po id
		if przedmiot 	# jezeli element znaleziono - wypozyczenie
			klient = klienci.find { |x| x.idKlienta==idKlienta}
			wypozyczenie = Wypozyczenie.new(przedmiot,idKlienta, klient)	# nowy obiekt wypozyczenia

			self.wypozyczone.push(wypozyczenie)	# dodanie wypozyczenia do listy wypozyczen
			self.przedmioty.delete(przedmiot)	# usuniecie przedmiotu z dostepnych przedmiotow
			return true # jezeli wypozyczone - zwrocenie true
		end
		false # jezeli wypozyczenie nieudane - zwrocenie false
	end

	def oddaj(idPrzedmiotu)
		indeks = self.wypozyczone.find_index {|w| w.idPrzedmiotu==idPrzedmiotu } # indeks przedmiotu w wypozyczonych

		if indeks # zwrocenie przedmiotu jesli znajduje sie on w wypozyczonych
			self.przedmioty.push(self.wypozyczone.at(indeks).przedmiot)
			self.wypozyczone.delete_at(indeks)
		end
	end

	def dodajPrzedmiot(przedmiot)
		przedmiot.idPrzedmiotu=@@id_przedmiotow
		self.przedmioty.push(przedmiot)
		@@id_przedmiotow = @@id_przedmiotow + 1
	end

	def usunPrzedmiot(idPrzedmiotu)
		indeks = self.przedmioty.find_index { |p| p.idPrzedmiotu == idPrzedmiotu }
		przedmioty.delete_at(indeks)
	end

	def dodajKlienta(nowyKlient)
		nowyKlient.idKlienta = @@id_klientow
		self.klienci.push(nowyKlient)
		@@id_klientow = @@id_klientow + 1 
	end

end

class Ksiazka

	attr_accessor :idPrzedmiotu

	def initialize(tytul, autor, rokWydania)
		@tytul=tytul
		@autor=autor
		@rokWydania=rokWydania
		@idPrzedmiotu
	end

	def info
		"id:" + @idPrzedmiotu.to_s+ " " + @tytul + " " + @autor + " " + @rokWydania 
	end
end

class Klient
	attr_accessor :idKlienta

	def initialize(imie,nazwisko,tel,adres)
		@imie=imie
		@nazwisko=nazwisko
		@tel=tel
		@adres=adres
		@idKlienta
	end

	def info
		"id:" + @idKlienta.to_s+ " " + @imie + " " + @nazwisko + " " + @tel + " " + @adres 
	end
end

class Wypozyczenie

	attr_accessor :klient, :idKlienta, :idPrzedmiotu, :przedmiot

	def initialize(przedmiot,idKlienta,klient)
		@przedmiot=przedmiot
		@idPrzedmiotu=@przedmiot.idPrzedmiotu
		@idKlienta=idKlienta
		@klient=klient
	end

	def info
		"Klient " + @klient.info + " wypozyczyl ksiazke: " + przedmiot.info
	end
end

class Biblioteka

	include Wypozyczalnia
	attr_accessor :przedmioty, :wypozyczone, :klienci
	def initialize
		@przedmioty = Array.new()
		@wypozyczone = Array.new()
		@klienci = Array.new()
	end

	def wypiszKlientow
		klienci.each { |k| puts k.info; puts "***" }
	end

	def wypiszKsiazki
		przedmioty.each { |k| puts k.info; puts "***"}
	end

	def wypiszWypozyczone
		wypozyczone.each { |k| puts k.info; puts "***" }
	end
end

b = Biblioteka.new

b.dodajKlienta(Klient.new("Jan","Kowalski","547-455-235","Wrocław, ul. Wyszynskiego 4"))
b.dodajKlienta(Klient.new("Adam","Strzelec","558-485-235","Wrocław, ul. Powstancow 45"))
b.dodajKlienta(Klient.new("Maria","Nowak","647-121-235","Wrocław, ul. Nowowiejska 30"))
b.dodajKlienta(Klient.new("Ewelina","Bury","847-956-235","Wrocław, ul. Ladna 4"))
b.dodajKlienta(Klient.new("Maciej","Gruza","455-35-23","Wrocław, ul. Wyszynskiego 100"))
b.dodajKlienta(Klient.new("Dominik","Dziura","888-435-235","Wrocław, ul. Wyszynskiego 4"))
b.dodajKlienta(Klient.new("Marcin","Barteczko","868-435-235","Wrocław, ul. Wyszynskiego 4"))
puts "Klienci:"
b.wypiszKlientow

b.dodajPrzedmiot(Ksiazka.new("Pan Tadeusz","Adam Mickiewicz", "1987"))
b.dodajPrzedmiot(Ksiazka.new("Potop","Henryk Sienkiewicz", "1990"))
b.dodajPrzedmiot(Ksiazka.new("Programowanie w języku Ruby","Dave Thomas", "2000"))
b.dodajPrzedmiot(Ksiazka.new("Czysty kod","Adam Mickiewicz", "2014"))
b.dodajPrzedmiot(Ksiazka.new("Historia Polski","Jan Kowalski", "1999"))
b.dodajPrzedmiot(Ksiazka.new("Zbior wierszy","Rozni", "1900"))
puts " "
puts "Ksiazki:"
b.wypiszKsiazki

b.wypozycz(0,0)
b.wypozycz(2,1)
b.wypozycz(3,3)
b.wypozycz(4,3)
b.wypozycz(5,6)

puts " "
b.wypiszWypozyczone

b.oddaj(0)
b.oddaj(4)

puts " "
puts "Po oddaniu :"

b.wypiszWypozyczone

b.usunPrzedmiot(0)
b.usunPrzedmiot(4)

puts " "
puts "Po usunieciu ksiazek ze zbioru: "
b.wypiszKsiazki