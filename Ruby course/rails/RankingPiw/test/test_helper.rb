ENV['RAILS_ENV'] ||= 'test'
require File.expand_path('../../config/environment', __FILE__)
require 'rails/test_help'

class ActiveSupport::TestCase
  # Setup all fixtures in test/fixtures/*.yml for all tests in alphabetical order.
  fixtures :all

  # Add more helper methods to be used by all tests here...
  include Sorcery::TestHelpers::Rails
def authenticate_user(user = users(:admin))
  post user_sessions_url, params: { email: user.email, password: "admin" } 
  assert_redirected_to users_path
  assert_equal session[:user_id].to_i, user.id
end

end
