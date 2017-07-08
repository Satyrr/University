##
# Vote columns:
# - t.float    "score"
# - t.integer  "beer_id"
class Vote < ApplicationRecord
	belongs_to :beer
end
