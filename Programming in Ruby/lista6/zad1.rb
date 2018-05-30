require 'dbi'
require 'sqlite3'

class CD_db
	include SQLite3
	def initialize(filename)
		@filename = filename
		@db = DBI.connect("DBI:SQLite3:#{filename}")

		create_tables 
		rescue DBI::DatabaseError => e
	     	puts "An error occurred"
	     	puts "Error code: #{e.err}"
	    	puts "Error message: #{e.errstr}"
   		
	end

	def create_tables

		@db.do("CREATE TABLE IF NOT EXISTS Plyty(\
		idPlyty INTEGER PRIMARY KEY,\
		idAutora INT NOT NULL,\
		idWypozyczenia INT,\
		nazwaPlyty VARCHAR(50))\
		")

		@db.do("CREATE TABLE IF NOT EXISTS Autorzy(\
		idAutora INTEGER PRIMARY KEY,\
		nazwaAutora VARCHAR(40) UNIQUE)\
		")

		@db.do("CREATE TABLE IF NOT EXISTS Utwory(\
		idUtworu INTEGER PRIMARY KEY,\
		nazwaUtworu VARCHAR(50),\
		idPlyty INT NOT NULL)\
		")

		@db.do("CREATE TABLE IF NOT EXISTS Wypozyczenia(\
		idWypozyczenia INTEGER PRIMARY KEY,\
		nazwaWypozyczajacego VARCHAR(40))\
		")
	end

	def show_db

	end

	def add_cd(cd_name, author, tracks)
		#sprawdzenie czy autor wystepuje w bazie
		s = @db.execute("SELECT idAutora FROM Autorzy WHERE nazwaAutora='#{author}'")

		author_id = nil
		s.each{|row| author_id=row[0]}
		if author_id==nil
			@db.do("INSERT INTO Autorzy(nazwaAutora) VALUES('#{author}') ")
			author_id=@db.func(:insert_id)
		end
		puts author_id

		#dodanie 
		#s = @db.execute("SELECT idAutora FROM Autorzy WHERE nazwaAutora='#{author}'")

	end

	def close_db
		if @db then @db.disconnect end
 	end

end

begin

b= CD_db.new("plyty.db")
b.add_cd("asdasd","sdasd12395","asd")

#ensure b.close_db

end