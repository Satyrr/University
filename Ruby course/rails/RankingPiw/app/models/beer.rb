##
# Beer columns:
# - t.string   "title"
# - t.text     "description"
# - t.string   "brewery"
# - t.integer  "type_id"
# - t.float    "alc"
# - t.string   "img"
# - t.float    "score"
class Beer < ApplicationRecord
	belongs_to :type
	has_many :comments, dependent: :destroy
	has_many :votes, dependent: :destroy

	validates :title, presence: true,
                    length: { minimum: 5 }
    validates :description, presence: true,
                    length: { minimum: 5 }
end
