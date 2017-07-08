require 'tk'

class Okienko
	
	def run

		#okno glowne
		@win = TkRoot.new { title 'Armata' }

		#kontrolki do wpisania kata i szybkosci
		@angle = TkLabel.new(@win) { text 'Kat(stopnie):'; grid('row' => 1, 'column' => 1) }
		@angle_entry = TkEntry.new(@win) { grid(:row => 1, :column => 2) }
		@speed = TkLabel.new(@win) { text 'Predkosc(m/s):'; grid('row' => 2, 'column' => 1) }
		@speed_entry = TkEntry.new(@win) { grid(:row => 2, :column => 2) }

		#canvas do wyrysowania armaty i toru lotu pocisku
		@cv = TkCanvas.new(@win) { width 500; height 500; grid(:row => 3, :column => 1) }

		#inicjalizacja armaty i celu
		random_target
		init_items

		@shoot = TkButton.new(@win) { text 'Strzał'; grid(:row => 4, :column => 2) }
		@shoot.command { self.shoot }
		TkButton.new(@win) { text 'KONIEC'; command { exit }; grid(:row => 5, :column => 2) }

		Tk.mainloop

	end

	def init_items
		# utworzenie prostokata w ktorym znajdzie sie armata i cel
		box = TkcRectangle.new(@cv, 20, 50, 499, 499)

		#utworzenie armaty
		cann = TkcLine.new(@cv, 30, 490, 50, 465)
		cann.width = 4
		cann_wheel = TkcArc.new(@cv, 25, 485, 35, 495)
		cann_wheel.start = 0
		cann_wheel.extent = 359
		cann_wheel.fill = 'black'

		#utworzenie celu
		r = @target_r 
		x = @target_x
		y = @target_y 
		target = TkcArc.new(@cv, x-r,y-r, x+r, y+r)
		target.start = 0
		target.extent = 359
		target.fill = 'black'
	end

	#wylosowanie pozycji celu
	def random_target
		@target_r = r = 20
		@target_x = x = 300 + rand(100)
		@target_y = y = 100 + rand(300)
	end

	def shoot

		@cv.delete("all")
		init_items
		#kat i predkosc poczatkowa
		angle = eval(@angle_entry.value)
		v0 = eval(@speed_entry.value)

		#predkosc pionowa i pozioma
		v_vert = v0 * Math::sin(angle*(Math::PI/180))
		v_hor = v0 * Math::cos(angle*(Math::PI/180))
		sleep_time = 0.2/v_hor

		#poczatkowe wspolrzedne pocisku
		x = 50 ; y = 465 

		#tor pocisku
		while y > 0 && x < 499 do 
			#obliczenie nastepnej wysokosci pocisku
			y_next = 465 - ball_height(v_vert, v_hor, (x-50)+1)
			#wyrysowanie pozycji pocisku
			line = TkcLine.new(@cv, x, y, x+1, y_next)

			y = y_next
			x += 1
			line.update
			sleep(sleep_time)
			#sprawdzenie czy trafiono w cel
			if x.between?(@target_x-@target_r,@target_x+@target_r) \
			 	&& y.between?(@target_y-@target_r,@target_y+@target_r)
			 	
			 	question = Tk.messageBox(
				  'type'    => "yesno",  
				  'icon'    => "info", 
				  'title'   => "Cel trafiony",
				  'message' => "Czy chcesz zagrac ponownie?"
				)

				if question == "no" then exit end
				#zainicjowanie nowej planszy
				@cv.delete("all")
				random_target
				init_items
				break
			end

		end 
	end	

	#wysokosc pocisku dla danego x
	def ball_height(v_vert, v_hor, x)
		t = x/v_hor
		return v_vert*t - 5*t*t
	end


end

class Mojaklasa
	def init
		@aaa = 3
	end

	def put
		puts @aaa
	end
	@aaa = 5
end

#Okienko.new.run

m = Mojaklasa.new
m.put