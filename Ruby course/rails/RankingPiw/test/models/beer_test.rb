require 'test_helper'
  
class BeerTest < ActiveSupport::TestCase
  #include Sorcery::TestHelpers::Rails::Integration
  #include Sorcery::TestHelpers::Rails::Controller

    test "shouldnt save beer without description or title" do
       beer = Beer.new(:description => "opisabc")
       beer2 = Beer.new(:title => "nazwa")
       beer_empty = Beer.new

       assert_not beer.save
       assert_not beer2.save
       assert_not beer_empty.save
     end

     test "shouldnt save beer with too short description or title" do
       beer = Beer.new(:title => "name", :description => "opis123")
       beer2 = Beer.new(:title => "name123", :description => "opis")
       beer3 = Beer.new(:title => "name", :description => "opis")

       assert_not beer.save
       assert_not beer2.save
       assert_not beer2.save
     end

     test "valid beer save" do 
     	beer = Beer.new(:title => "name123", :description => "opis123")

     	assert beer.save
     end



end
