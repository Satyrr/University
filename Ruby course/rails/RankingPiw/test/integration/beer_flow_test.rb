require 'test_helper'

class BeerFlowTest < ActionDispatch::IntegrationTest
  test "can create a beer" do
  	user = users(:one)
  	authenticate_user user
    get "/beers/new"
    assert_response :success
 
    post "/beers",
      params: { beer: { title: "Nowe piwo", description: "Opis piwa" } }
    assert_response :redirect
  end
end
