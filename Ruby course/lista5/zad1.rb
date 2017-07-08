require 'open-uri'

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
		yield page	# wywolanie bloku na aktualnej stronie

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
				info = info + $1 + "\n\n"
			end
		end

		if reg_title =~ page 
			info = info + "Tytul: " + $1
		end
		info
	end
end

=begin
block =  proc{ |s| puts Strony.new.page_summary(s) ; puts "Waga strony: " + Strony.new.page_weight(s).to_s }

Strony.new.przeglad("http://www.ii.uni.wroc.pl", 0, block)
=end
Strony.new.przeglad("http://www.ii.uni.wroc.pl", 0) { |s| puts Strony.new.page_summary(s) ; puts "Waga strony: " + Strony.new.page_weight(s).to_s }

[1..10].each {|x| puts x}