##
# Comments controller allows to comment beers and deleting comments (only 
# by users).
class CommentsController < ApplicationController
	before_filter :require_login, except: [:create]
  ##
  # Creates comment and redirects to beer's page.
  def create
    @beer = Beer.find(params[:beer_id])

    @comment = @beer.comments.create(comment_params)
    redirect_to beer_path(@beer)
  end
  
  ## 
  # Deletes chosen comment and redirects to beer's page.
  def destroy
  	@beer = Beer.find(params[:beer_id])
    @comment = @beer.comments.find(params[:id])
    @comment.destroy
    redirect_to beer_path(@beer)
  end
 
  private
    def comment_params
      params.require(:comment).permit(:commenter, :body)
    end
end
