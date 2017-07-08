require 'dbi'
require 'sqlite3'

class CD_db

	include SQLite3
	def initialize(filename)
		@filename = filename
		@db = Database.new(filename)
		@db.results_as_hash = true	#umozliwienie odwolywania sie do kolumn poprzez hashe

		create_tables 
		rescue DBI::DatabaseError => e
	     	puts "An error occurred"
	     	puts "Error code: #{e.err}"
	    	puts "Error message: #{e.errstr}"
   		
	end

	def create_tables
		#tabela plyt
		@db.execute("CREATE TABLE IF NOT EXISTS Plyty(\
		idPlyty INTEGER PRIMARY KEY,\
		idAutora INT NOT NULL,\
		idWypozyczenia INT,\
		nazwaPlyty VARCHAR(50))\
		")
		#tabela autorow
		@db.execute("CREATE TABLE IF NOT EXISTS Autorzy(\
		idAutora INTEGER PRIMARY KEY,\
		nazwaAutora VARCHAR(40) UNIQUE)\
		")
		#tabela utworow
		@db.execute("CREATE TABLE IF NOT EXISTS Utwory(\
		idUtworu INTEGER PRIMARY KEY,\
		nazwaUtworu VARCHAR(50),\
		idPlyty INT NOT NULL)\
		")
		#tabela wypozyczen
		@db.execute("CREATE TABLE IF NOT EXISTS Wypozyczenia(\
		idWypozyczenia INTEGER PRIMARY KEY,\
		nazwaWypozyczajacego VARCHAR(40))\
		")
	end

	def show_db
		cds=@db.execute("SELECT * FROM Plyty")

		cds.each do |cd_row|
			puts "Nazwa plyty: " + cd_row['nazwaPlyty'] + " id:" + cd_row['idPlyty'].to_s

			#zapytanie o autora danej plyty
			author = @db.get_first_value("SELECT nazwaAutora FROM Autorzy\
										 WHERE idAutora=#{cd_row['idAutora']}")
			puts "Autor: #{author}, id:" + cd_row['idAutora'].to_s

			#utwory danej plyty
			tracks = @db.execute("SELECT nazwaUtworu FROM Utwory\
								 WHERE idPlyty=#{cd_row['idPlyty']}")
			i=1
			tracks.each {|track| puts i.to_s + ". " + track[0];i=i+1}

			#ewentualny wypozyczajacy danej plyty
			if cd_row['idWypozyczenia']!=nil 
			lender = @db.get_first_value("SELECT nazwaWypozyczajacego FROM Wypozyczenia\
										WHERE idWypozyczenia=#{cd_row['idWypozyczenia']}")
			puts "Wypozyczone przez: " + lender
			end	

			puts "\n\n"
		end
	end

	def search(string)
		#wypisanie wszystkich utworow, ktorych nazwy autora, plyty lub samego utworu zwieraja
		#string

		#szukanie autorow
		found = @db.execute("SELECT * FROM Autorzy WHERE nazwaAutora LIKE '%#{string}%'")
		found.each do |row|
			puts "Autor: #{row['nazwaAutora']}"
			cds=@db.execute("SELECT * FROM Plyty WHERE idAutora=#{row['idAutora']}")
			cds.each do |cd|
				tracks=@db.execute("SELECT * FROM Utwory WHERE idPlyty=#{cd['idPlyty']}")
				tracks.each {|track| puts "Utwor:" + track['nazwaUtworu'] }
			end
			
		end

		#szukanie plyt
		found = @db.execute("SELECT * FROM Plyty WHERE nazwaPlyty LIKE '%#{string}%' ")
		found.each do |row|
				puts "Plyta: #{row['nazwaPlyty']}"
				tracks=@db.execute("SELECT * FROM Utwory WHERE idPlyty=#{row['idPlyty']}")
				tracks.each {|track| puts "Utwor:" + track['nazwaUtworu'] }
		end

		#szukanie utworow
		tracks=@db.execute("SELECT * FROM Utwory WHERE nazwaUtworu LIKE '%#{string}%'")
		tracks.each {|track| puts "Utwor:" + track['nazwaUtworu'] }


 	end


	def add_cd(cd_name, author, tracks)
		#sprawdzenie czy autor wystepuje w bazie
		s = @db.execute("SELECT idAutora FROM Autorzy WHERE nazwaAutora='#{author}'")
		author_id = nil
		s.each{|row| author_id=row[0]} #wyznaczenie id autora 
		if author_id==nil
			@db.execute("INSERT INTO Autorzy(nazwaAutora) VALUES('#{author}') ") #dodanie nowego autora
			author_id=@db.last_insert_row_id()
		end
		
		#dodanie plyty do tabely Plyty
		@db.execute("INSERT INTO Plyty(idAutora,nazwaPlyty) VALUES(#{author_id},'#{cd_name}')")
		cd_id = @db.last_insert_row_id()

		#dodanie wszystkich utworow
		tracks.each do |track|
			@db.execute("INSERT INTO Utwory(idPlyty,nazwaUtworu) VALUES(#{cd_id},'#{track}')")
		end
	end

	def remove_cd(cd_id)
		@db.execute("DELETE FROM Plyty WHERE idPlyty=#{cd_id}")

		@db.execute("DELETE FROM Utwory WHERE idPlyty=#{cd_id}")
	end



	def lend(cd_id, lender_name)
		#sprawdzenie czy plyta o danym id istnieje w bazie
		i=-1
		@db.execute("SELECT * FROM Plyty WHERE idPlyty=#{cd_id}").each { i=0 }
		if i==-1 then return end

		#dodanie wypozyczenia
		@db.execute("INSERT INTO Wypozyczenia(nazwaWypozyczajacego)\
					VALUES('#{lender_name}')")

		#id dodanego wypozyczenia
		id=@db.last_insert_row_id
		@db.execute("UPDATE Plyty SET idWypozyczenia=#{id} WHERE idPlyty=#{cd_id}")
	end

	def get_cd_back(cd_id)
		id=@db.get_first_value("SELECT idWypozyczenia FROM Plyty WHERE idPlyty=#{cd_id}")

		@db.execute("UPDATE Plyty SET idWypozyczenia=NULL WHERE idPlyty=#{cd_id}")
		@db.execute("DELETE FROM Wypozyczenia WHERE idWypozyczenia=#{id}")
	end



	def close_db
		if @db then @db.disconnect end
 	end


end


b= CD_db.new("plyty.db")

=begin
tracks = "I Cant Stop Thinking About You/50,000/Down, Down, Down\
		  /One Fine Day/Pretty Young Soldier/Petrol Head\
		  /Heading South On The Great North Road/If You Cant Love Me".split('/')

tracks2 = "Hardwired/Atlas, Rise!/Now That Were Dead/Moth Into Flame/Dream No More\
		  /Halo On Fire/Confusion/Manunkind/Here Comes Revenge/Am I Savage".split('/')

b.add_cd("Sting","57th 9th",tracks)
b.add_cd("	Enigma","Brotherhood Of The Snake",tracks2)
=end

option=1
options = "1.Wyswietl baze
2.Dodaj plyte
3.Usun plyte
4.Wypozycz
5.Odzyskaj plyte
6.Wyszukaj"
cd_info="Podaj nazwe plyty, jej autora, utwory, a nastepnie wpisz 0 i zatwierdz"

foo=""
while option>0 do
	puts options
	option = gets.chomp.to_i
	case option
		when 1
			b.show_db
			puts "Wybierz ponownie..."
		when 2
			puts cd_info
			cd_name = gets.chomp
			cd_author = gets.chomp 
			tracks = Array.new
			track_name=""
			while track_name!="0" do
				track_name = gets.chomp 
				tracks << track_name
			end
			b.add_cd(cd_name,cd_author,tracks)
			puts "Wybierz ponownie..."
		when 3
			puts "Podaj id plyty"
			id = gets.chomp.to_i
			b.remove_cd(id)
			puts "Wybierz ponownie..."
		when 4
			puts "Podaj imie wypozyczajacego:"
			first_name = gets.chomp 
			puts "Podaj nazwisko wypozyczajacego"
			surname = gets.chomp 
			puts "Podaj id plyty do wypozyczenia:"
			cd_id = gets.chomp.to_i
			b.lend(cd_id,first_name + " " + surname)
			puts "Wybierz ponownie..."
		when 5
			puts "Podaj id plyty"
			id = gets.chomp 
			b.get_cd_back(id)
			puts "Wybierz ponownie..."
		when 6 
			puts "Podaj haslo do wyszukania"
			str = gets.chomp 
			b.search(str)
			puts "Wybierz ponownie..."
		else
			puts "zla opcja, wybierz ponownie"
	end
end

