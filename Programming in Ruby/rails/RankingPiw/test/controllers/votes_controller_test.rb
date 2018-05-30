require 'test_helper'

class VotesControllerTest < ActionDispatch::IntegrationTest
  setup do
    @beer = beers(:one)
    @vote = votes(:one)
  end

  test "should create vote" do
    assert_difference('Vote.count') do
      post beer_votes_url(@beer), params: { vote: { score: 4.4, beer_id: 1 } }
    end

    assert_redirected_to beer_url(@beer)
  end


end
