class CreateBeers < ActiveRecord::Migration[5.0]
  def change
    create_table :beers do |t|
      t.string :title
      t.text :description
      t.string :brewery
      t.references :type
      t.float :alc
      t.string :img

      t.timestamps
    end
  end
end
