require 'test_helper'

class TypeTest < ActiveSupport::TestCase

	   test "shouldnt save type without title" do
       type_empty = Type.new

       assert_not type_empty.save
     end

     test "shouldnt save type with too short title" do
       type = Type.new(:title => "name")

       assert_not type.save
     end

     test "valid type save" do 
     	type = Type.new(:title => "name123")

     	assert type.save
     end

end
