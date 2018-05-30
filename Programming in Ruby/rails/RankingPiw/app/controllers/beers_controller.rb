##
# Beers controller with standard CRUD operations and ranking action.
# All actions, except ranking and show, require to be logged in.
class BeersController < ApplicationController
	before_filter :require_login, except: [:ranking, :show]

  ##
  # Index with all beers. Users can maintain beers' database here.
  # routes:
  # - /beers
  def index
    @beers = Beer.all
  end

  ##
  # Ranking is index ordered by scores.It can be seen by everyone.
  # routes:
  # - /ranking
  def ranking
    @beers = Beer.all
    @beers = @beers.order(score: :desc)
  end

  ##
  # Add new beer form.
  # @options is array with types of beer.
  # routes:
  # - /beers/new
  def new
  	@beer = Beer.new
  	
  	@options = Array.new 
	get_options

  end
  
  ##
  # Show chosen beer.
  # routes:
  # * /beers/:id
  def show
  	@beer = Beer.find(params[:id])
  	@scores = Array.new
  		for i in 1..10
  			@scores << [i,i]
  		end
  end

  ##
  # Create new beer.
  # routes:
  # * /beers
  def create
    @beer = Beer.new(beer_params)

	if @beer.save
      redirect_to @beer
    else
      @options = Array.new
      get_options
      render 'new'
    end
  end

  ##
  # Destroy selected beer form.
  # routes:
  # * /beers/:id
  def destroy
  	@beer = Beer.find(params[:id])
  	@beer.destroy

  	redirect_to beers_path
  end
  
  ##
  # Edit selected beer.
  # @options is array with types of beer.
  # routes:
  # * /beers/:id/edit
  def edit
  	@beer=Beer.find(params[:id])

  	@options = Array.new 
	get_options
  end
  
  ##
  # Update edited beer.
  # @options is array with types of beer.
  # routes:
  # * /beers/:id
  def update
  	@beer=Beer.find(params[:id])

  	if @beer.update(beer_params)
      redirect_to @beer
    else
      @options = Array.new
      get_options
      render 'edit'
    end
  end
  
  ##
  # Creates @options array with beers' types.
  def get_options
  	@types = Type.all
  	@types.each do |type|
      @options << [type[:title],type[:id]] 
    end
  end

  private
  def beer_params
      params.require(:beer).permit(:title, :description, :brewery, :type_id, :alc, :img, :score)
  end
end
