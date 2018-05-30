require 'date'

zadania = []
konflikty = []
id = 0

def pierwszyZnak(str)
    
    if str.length == 0 then
            return nil
    end
    i=0
    while str[i].match(/\s/)
        i+=1
        if str[i] == nil then
            return nil
        end
    end
    return str[i]
end
        
def dodajZadanie(zadania,id)
    
    termin = pobierzTermin
    puts "Podaj nazwe spotkania:"
    nazwa = gets
    
    zadania << [termin, nazwa,id]
    zadania.sort! {|x,y| x[0] <=> y[0]}
    id += 1
end
        
def pobierzTermin
    puts "Podaj dzien miesiaca:"
    dzien = gets.to_i
    puts "Podaj miesiac:"
    miesiac = gets.to_i
    puts "Podaj rok:"
    rok = gets.to_i
    puts "Podaj godzine:"
    godzina = gets.to_i
    puts "Podaj minute:"
    minuta = gets.to_i
    
    DateTime.new(rok,miesiac,dzien,godzina,minuta,0)
end
        
def usunZadanie(id, zadania)
    zadania.delete_if {|zadanie| zadanie[2] == id}
end
        
def sprawdzKonflikty(zadania)
    konflikty = []
    zadania.each {|zadanie1| 
            zadania.each {|zadanie2| 
                roznica = (zadanie2[0]-zadanie1[0])*24*60*60
                if(roznica.to_i>=0 && roznica.to_i<3600 && zadanie1[2]!=zadanie2[2]) 
                    konflikty << [zadanie1[2],zadanie2[2]]
                end
            }
        }
    konflikty
end
        
def wypiszZadania(zadania)
   zadania.each {|zadanie| print "id: " + zadanie[2].to_s + " " + zadanie[0].to_s + " " + zadanie[1] } 
end

        
################### Program glowny ###################
        
opcja = nil

while opcja != "3" do
opcja = nil
menu = <<END_OF_STRING
Wybierz opcje:
(1) Dodaj spotkanie
(2) Usun spotkanie
(3) Zakoncz
***********************
END_OF_STRING
puts menu
puts "Spotkania i zadania:"
wypiszZadania(zadania)
    
konflikty = sprawdzKonflikty(zadania)    
if konflikty
   konflikty.each {|id1,id2| puts "Uwaga! Konflikt pomiędzy spotkaniami o id " + id1.to_s + " oraz " + id2.to_s} 
end
    
while !(["1","2","3"].include? opcja )
    opcja = pierwszyZnak(gets)
end
    
if opcja == "1"
    id=dodajZadanie(zadania,id)
end
if opcja == "2"
    puts "Podaj nr id zadania do usuniecia:"
    usunZadanie(gets.to_i, zadania)
end

system "clear" or system "cls"
end