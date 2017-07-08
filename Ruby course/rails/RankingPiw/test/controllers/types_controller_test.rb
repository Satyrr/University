require 'test_helper'

class TypesControllerTest < ActionDispatch::IntegrationTest
  setup do
    @type = types(:one)
    @user = users(:one)

    authenticate_user(@user) 
  end

  test "should get index" do
    get types_url
    assert_response :success
  end

  test "should get new" do
    get new_type_url
    assert_response :success
  end

  test "should create type" do
    assert_difference('Type.count') do
      post types_url, params: { type: { title: "nazwa" } }
    end

    assert_redirected_to types_url
  end

  test "should get edit" do
    get edit_type_url(@type)
    assert_response :success
  end

  test "should update type" do
    patch type_url(@type), params: { type: { title: "nazwa2" } }
    assert_redirected_to types_url
  end

  test "should destroy type" do
    assert_difference('Type.count', -1) do
      delete type_url(@type)
    end

    assert_redirected_to types_url
  end
end
