##
# Comment columns:
# - t.string   "commenter"
# - t.text     "body"
# - t.integer  "beer_id"

class Comment < ApplicationRecord
  belongs_to :beer
end
