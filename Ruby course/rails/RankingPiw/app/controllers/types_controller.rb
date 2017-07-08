##
# Types controller allows to adding, editing ale deleting beers' types by users.
# They can also be seen by non logged viewers.
class TypesController < ApplicationController
	before_filter :require_login, except: [:index]
  
  ##
  # Index for all types. Anyone can see them.
  # routes :
  # - /types
  def index
  	beer = Beer.new
  	beer.save
  	@types = Type.all
  end 

  ##
  # New type's form.
  # routes :
  # - /types/new
  def new
  	@type = Type.new
  end
  
  ##
  # Creates new type. Redirects to type's index.
  # routes :
  # - /types
  def create
    @type = Type.new(type_params)

	if @type.save
      redirect_to types_path
    else
      render 'new'
    end
  end
  
  ##
  # Destroy chosen type. Redirects to types' index.
  # routes :
  # - /types/id
  def destroy
  	@type = Type.find(params[:id])
  	@type.destroy

  	redirect_to types_path
  end

  ##
  # Edit chosen type.
  # routes :
  # - /types/:id
  def edit
  	@type=Type.find(params[:id])
  end

  ##
  # Update chosen type. Redirects to types' index. Renders edit form if 
  # update failed.
  # routes :
  # - /types/:id
  def update
  	@type=Type.find(params[:id])

  	if @type.update(type_params)
      redirect_to types_path
    else
      render 'edit'
    end
  end

  private
  def type_params
      params.require(:type).permit(:title)
  end
end
