##
# User columns:
# - t.string   "username",         null: false
# - t.string   "email",            null: false
# - t.string   "crypted_password", null: false
# - t.string   "salt",             null: false

class User < ApplicationRecord
  authenticates_with_sorcery!
   validates_confirmation_of :password, message: "should match confirmation", if: :password
end
