##
# Controller to create session of user.
class UserSessionsController < ApplicationController
  ##
  # Login form
  # routes :
  # - /login
  # - /user_sessions/new
  def new
    @user = User.new
  end 

  ##
  # Creates new login session.
  # routes :
  # - /user_sessions
  def create
    if @user = login(params[:email], params[:password])
      redirect_back_or_to(:users, notice: 'Login successful')
    else
      flash.now[:alert] = 'Login failed'
      render action: 'new'
    end
  end

  ##
  # Destroys login session(logout).
  # routes :
  # - /user_sessions/:id
  def destroy
    logout
    redirect_to(:users, notice: 'Logged out!')
  end
end