require 'open-uri'
require 'monitor'

$lock = Monitor.new

class Strony

	def initialize
		@visited = Array.new
	end

	def fetch_site(link)
		page = ""
		open(link) do |fh| # otwarcie strony
			fh.each_line {|line| page = page + line} # skopiowanie strony do stringa
		end
		return page
	rescue 
	end

	def multi_przeglad pages, depth, &block

		threads = []


		pages.each do |p|
			threads << Thread.new { przeglad p, depth, &block }
		end
		threads.each {|t| t.join }
	end 

	def przeglad start_page, depth

		if start_page[start_page.length-1] == '/' 	# usuniecie ostatniego ukosnika w linku jesli istnieje
			start_page = start_page.chop
		end
		
		if @visited.include?(start_page) then return end 		# powrot jezeli strona zostala juz odwiedzona
		@visited.push(start_page)
		#puts start_page

		if depth == -1 then return end 	# maksymalna glebokosc

		page = fetch_site(start_page) 	# pobranie strony do stringa
		if !page then return end

		$lock.synchronize { yield page }	# wywolanie bloku na aktualnej stronie

		hrefs = Array.new	 # tablica a linkami do podstron
		page.each_line do |line| 	# wyszukanie linkow w liniach strony

			reg = /(<a\s+href="\s*)(\S+)(\s*")/
			if reg =~ line
				href = $2
				hrefs.push(href)
			end

			hrefs.map! { |href| if href.strip[0]=='/' then start_page + href else href end }
			hrefs.each { |href| przeglad(href,depth-1) }
		end 

	end

	def page_weight(page)
		reg = /(<img)|(<applet)|(<canvas)|(<embed)/
		page.scan(reg).length
	end

	def page_summary(page)
		info = ""
		reg_name = /<meta(.+)name\s*=\s*"(\S+)"(.+)/
		reg_content = /content="(.+)"/
		reg_title = /<title>(.+)<\/title>/

		metas = page.scan(reg_name)
		metas.each do |meta|
			info = info + meta[1] + " = "
			string = meta[0] + " " + meta[2]
			if reg_content =~ string
				info = info + $1 + "\n"
			end
		end

		if reg_title =~ page 
			info = info + "Tytul: " + $1
		end
		info = info + "\n******************************\n"
		info
	end
end

=begin
block =  proc{ |s| puts Strony.new.page_summary(s) ; puts "Waga strony: " + Strony.new.page_weight(s).to_s }

Strony.new.przeglad("http://www.ii.uni.wroc.pl", 0, block)
=end
#Strony.new.przeglad("http://www.ii.uni.wroc.pl", 0) { |s| puts Strony.new.page_summary(s) ; puts "Waga strony: " + Strony.new.page_weight(s).to_s }
#Strony.new.przeglad("http://www.ii.uni.wroc.pl", 0) { |s| puts Strony.new.page_summary(s) ; puts "Waga strony: " + Strony.new.page_weight(s).to_s }



adresy = [ "http://www.ii.uni.wroc.pl", "http://miroslawzelent.pl/", "https://www.wykop.pl/", "http://wroclaw.fotopolska.eu/" ]

Strony.new.multi_przeglad(adresy, 0) { |s| puts "Waga strony: " + Strony.new.page_weight(s).to_s ; puts Strony.new.page_summary(s) ; }