require 'date'

id=0
contacts = []
groups = []

def firstChar(str)
    
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
        
def addContact(contacts, groups)
    puts "Podaj imie:"
    name = gets.chomp
    puts "Podaj nazwisko:"
    surname = gets.chomp
    puts "Podaj numer:"
    phone = gets.chomp
    
    puts "Wybierz numery grup (0 - koniec)"
    writeGroups(groups)
    
    number = gets.to_i
    contactGroups = []
    
    while number != 0
        contactGroups << groups[number-1]
        puts "Dodano do grupy " + groups[number-1]
        number = gets.to_i
    end
    contacts << [name,surname,phone,contactGroups]
end
        
def addGroup(groups,groupName)
    if !groups.include? groupName
        groups << groupName 
    end
    groups
end
        
def searchContact(contacts, part)
    foundContacts = []
    contacts.each do | contact |
        for i in 0..2
            if contact[i].include? part
               foundContacts << contact 
            end
        end
    end
    foundContacts
end
        
def contactsOfGroup(groups, contacts)
    puts "Podaj grupe do wyswietlenia:"
    writeGroups(groups)
    number = gets.to_i-1
    
    foundContacts = []
    contacts.each do | contact |
         if contact[3].include? groups[number]
            foundContacts << contact
         end
    end
    foundContacts
end
        
def writeGroups(groups)
    i=1
    groups.each {|group| puts i.to_s + "." + group; i+=1}
end
        
def writeContacts(contacts)
    i = 1
    contacts.each do | contact |
        str = (i.to_s + "." + contact[0] + " " +  contact[1] + " Tel.nr: " +contact[2].to_s)
        puts str
        i+=1
    end
end
        
################### Program glowny ###################
    
option = nil

while option != "6" do
option = nil
menu = <<END_OF_STRING
Wybierz opcje:
(1) Dodaj kontakt
(2) Dodaj grupe
(3) Wyszukaj kontakt 
(4) Wyszukaj grupy
(5) Wyswietl wszystkie kontakty
(6) Zakoncz
***********************
END_OF_STRING
    
puts menu
while !((1..6).to_a.include? option.to_i )
    option = firstChar(gets)
end
case option
    when "1" 
        contacts=addContact(contacts, groups)
    when "2" 
        puts "Podaj nazwe grupy:"
        groupName = gets.chomp
        groups=addGroup(groups,groupName)
    when "3" 
        puts "Podaj czesc nazwy kontaktu lub numeru telefonu:"
        part = gets.chomp
        writeContacts(searchContact(contacts,part))
    when "4"
        writeContacts(contactsOfGroup(groups,contacts))
    when "5"
        writeContacts(contacts)
end
puts "***********************"
end