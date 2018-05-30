require 'test_helper'

class BeersControllerTest < ActionDispatch::IntegrationTest
   include Sorcery::TestHelpers::Rails::Integration
   include Sorcery::TestHelpers::Rails::Controller
   include Sorcery::Controller

   setup do
    @user = users(:one)
    @beer = beers(:one)

    authenticate_user(@user) 
  end

   test "should get index" do
    get beers_url
    assert_response :success
  end

  test "should get new beer" do
    get new_beer_url
    assert_response :success
  end

  test "should show beer" do
    get beer_url(@beer)
    assert_response :success
  end

  test "should get edit" do
    get edit_beer_url(@beer)
    assert_response :success
  end

  test "should update beer" do
    patch beer_url(@beer), params: { beer: { title: "nowa nazwa", description: "nowy opis" } }
    assert_redirected_to beer_url(@beer)
  end

  test "should destroy beer" do
    assert_difference('Beer.count', -1) do
      delete beer_url(@beer)
    end

    assert_redirected_to beers_url
  end
end
