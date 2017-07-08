# server.rb
require 'drb/drb'
require 'sqlite3'

class Server

	include SQLite3

	#uruchomienie serwera
	def Server.run
		@@server = Server.new
		@@db = Database.new("baza.db")
		@@db.results_as_hash = true	#umozliwienie odwolywania sie do kolumn poprzez hashe
		@@server.create_table

		@@id = -1 # zainicjalizowanie id

		Thread.new {@@server.menu_thread} # utworzenie watku menu
		DRb.start_service('druby://localhost:9999', @@server)
		DRb.thread.join
	end

	#utworzenie tabeli wiadomosci
	def create_table
		@@db.execute("CREATE TABLE IF NOT EXISTS Logs(\
		logId INTEGER PRIMARY KEY,\
		prgId INTEGER,\
		date INT NOT NULL,\
		log VARCHAR(100))\
		")
	end

	#metoda nadajaca id klientom
	def assign_id
		@@id=@@id+1
		@@id
	end

	def menu_thread
		menu = "1.wypisz raport\n"
		puts menu
		option=gets.chomp.to_i
		while 1 do
			if option == 1
				@@server.raport(Time.now-10000,Time.now,0,/ruby/)
			end
			option=gets.chomp.to_i
		end
	end

	#metoda wywolywana przez klienta o id prg_id, dodajaca wiadomosc msg do bazy
	def save(prg_id, msg)
		time = Time.now.to_i 
		@@db.execute("INSERT INTO Logs VALUES(NULL,#{prg_id},#{time},'#{msg}')")
	end

	#przeszukanie wiadomosci z zakresu dat: (from,to), wyslanych z klienta o id prg_id
	# i zawierajacych wyrazenie regularne re
	def raport(from, to, prg_id, re)
		logs = Array.new
		fetchedLogs = @@db.execute("SELECT prgId,date,log FROM Logs WHERE prgId=#{prg_id}")
		
		#zawezenie wiadomosci ze wzgledu na date
		fetchedLogs.each do |row|
			if row['date'] >= from.to_i && row['date'] <= to.to_i 
				logs.push ([row['prgId'],row['date'],row['log']])
			end
		end

		logs.each {|row| row[2] =~ re} # wyszukanie wyrazenia regularnego w wiadomociach
		@@server.make_html_raport(logs) #utworzenie raportu w html

	end

	def make_html_raport(logs)

		html = "<html>\n"\
		"<body>\n"\
		"<table>\n"\
		"<tr>\n"\
		"<th>ID Programu</th><th>Data</th><th>Wiadomosc</th>"\
		"</tr>\n"

		logs.each do |row|
			date = Time.at(row[1]) # utworzenie daty z integer
			html = html + "<tr>\n<td>#{row[0]}</td><td>#{date}</td><td>#{row[2]}</td>\n</tr>"
		end
		html = html + "</table>\n</body>\n</html>"

		File.open("raport.html","w") do |f|
			f.puts html
		end
	end
end

Server.run

