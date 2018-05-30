class AddScoreToBeers < ActiveRecord::Migration[5.0]
  def change
    add_column :beers, :score, :float
  end
end
