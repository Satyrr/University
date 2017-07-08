Rails.application.routes.draw do

	get 'ranking' => 'beers#ranking'
	resources :beers do 
		resources :comments
		post 'votes' => "votes#create"
	end
	resources :types
	resources :users
	root 'beers#ranking'

	resources :user_sessions, only: [ :new, :create, :destroy ]
	get 'login'  => 'user_sessions#new'
	get 'logout' => 'user_sessions#destroy'

  # For details on the DSL available within this file, see http://guides.rubyonrails.org/routing.html
end
