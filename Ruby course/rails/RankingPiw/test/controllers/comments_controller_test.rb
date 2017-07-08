require 'test_helper'

class CommentsControllerTest < ActionDispatch::IntegrationTest
  setup do
    @comment = comments(:one)
    @beer = beers(:one)
  end

  test "should create comment" do
    assert_difference('Comment.count') do
      post beer_comments_url(@beer), params: { comment: { commenter: "aaaaaa", body: "bbbb" } }
    end

    assert_redirected_to beer_url(@beer)
  end

  test "should destroy comment" do
    @user = users(:one)
    authenticate_user(@user) 

    assert_difference('Comment.count', -1) do
      delete beer_comment_url(@beer, @comment)
    end

    assert_redirected_to beer_url(@beer)
  end

  test "shouldnt destroy comment if no authenticated" do
    assert_no_difference('Comment.count', -1) do
      delete beer_comment_url(@beer, @comment)
    end

    assert_redirected_to root_url
  end
end
