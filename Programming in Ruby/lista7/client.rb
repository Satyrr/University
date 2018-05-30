require 'drb/drb'



SERVER_URI="druby://localhost:9999"

#ustanowienie polaczenia i przyznanie id
DRb.start_service
log_service=DRbObject.new_with_uri(SERVER_URI)
my_id = log_service.assign_id

menu = "1.nowa wiadomosc\n"\
"0.zamknij klienta"
puts menu
option=gets.chomp.to_i

while option!=0 do
	if option == 1
		puts "Podaj wiadomosc:\n"
		msg = gets.chomp
		if log_service.respond_to?('save')
			log_service.save(my_id,msg) # wyslanie wiadomosci
		end
	end
	puts menu
	option=gets.chomp.to_i
	
end