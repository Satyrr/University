##
# Votes system for beers.
class VotesController < ApplicationController
  ##
  # Creates new vote for beer. Redirects to that beer.
  # routes:
  # -/beers/:beer_id/votes
  def create
    @beer = Beer.find(params[:beer_id])
    @vote = @beer.votes.create(vote_params)
    sum,n = 0.0,0
    @beer.votes.each do |vote|
    	sum+=vote[:score]
    	n+=1
    end
    @beer.score=(sum/n).round(2)
    @beer.save

    redirect_to beer_path(@beer)
  end
 
  private
    def vote_params
      params.require(:vote).permit(:score)
    end

end
