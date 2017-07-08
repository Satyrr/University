##
# Type columns:
# - t.string   "title"

class Type < ApplicationRecord
	has_many :beers
	validates :title, presence: true,
                    length: { minimum: 5 }
end
