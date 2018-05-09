import random

MEADOW = 0
TRAP = 1
MERE = 2
LOWERCAVE = 3
UPPERCAVE = 4
jungle_map_dict = {0:'.', 1:'#', 2:'-', 3:'*', 4:'*'}


RAT = 0
CAT = 1
DOG = 2
WOLF = 3
PANTHER = 4
TIGER = 5
LION = 6
ELEPHANT = 7
animals_dict = {None:'.', 0:'r', 1:'c', 2:'d', 3:'w', 4:'j', 5:'t', 6:'l', 7:'e'}

MIN = 0
MAX = 1

jungle_map = [[MEADOW]*7 for _ in range(9)]
jungle_map[0][2] = jungle_map[1][3] = jungle_map[0][4] = TRAP
jungle_map[8][2] = jungle_map[7][3] = jungle_map[8][4] = TRAP
for i in range(3,6):
	jungle_map[i][1] = jungle_map[i][2] = jungle_map[i][4] = jungle_map[i][5] = MERE
jungle_map[0][3] = UPPERCAVE
jungle_map[8][3] = LOWERCAVE

dirs = [(-1,0), (0,1), (1,0), (0,-1)]

class Animal:

	def __init__(self, type, x, y, player):
		self.type = type
		self.x = x
		self.y = y
		self.player = player

	def clone(self):
		return Animal(self.type, self.x, self.y, self.player)

class JungleGame:
	@staticmethod
	def draw_jungle_map():
		string_map = '\n'.join([''.join([jungle_map_dict[elem] for elem in row]) for row in jungle_map])
		print(string_map)
		print('\n')

	def __init__(self, proto_game=None):
		if proto_game == None:
			self.max_animals = self.init_animals(MAX)
			self.min_animals = self.init_animals(MIN)
			self.animal_map = [[None] * 7 for _ in range(9)]
			self.init_animal_map()
			self.passive_moves = 0
			self.moves_number = 0
			self.player = MIN if random.random() < 0.5 else MAX
		else:
			self.max_animals = [an.clone() for an in proto_game.max_animals]
			self.min_animals = [an.clone() for an in proto_game.min_animals]
			self.animal_map = [[None] * 7 for _ in range(9)]
			self.init_animal_map()
			self.passive_moves = proto_game.passive_moves
			self.player = proto_game.player
			self.moves_number = proto_game.moves_number

	def init_animals(self, player):
		if player == MAX:
			return [
				Animal(RAT, 2, 0, player),
				Animal(CAT, 1, 5, player),
				Animal(DOG, 1, 1, player),
				Animal(WOLF, 2, 4, player),
				Animal(PANTHER, 2, 2, player),
				Animal(TIGER, 0, 6, player),
				Animal(LION, 0, 0, player),
				Animal(ELEPHANT, 2, 6, player)
			]
		else:
			return [
				Animal(RAT, 6, 6, player),
				Animal(CAT, 7, 5, player),
				Animal(DOG, 7, 1, player),
				Animal(WOLF, 6, 2, player),
				Animal(PANTHER, 6, 4, player),
				Animal(TIGER, 8, 0, player),
				Animal(LION, 8, 6, player),
				Animal(ELEPHANT, 6, 0, player)
			]

	def init_animal_map(self):
		for animal in self.max_animals:
			self.animal_map[animal.x][animal.y] = animal
		for animal in self.min_animals:
			self.animal_map[animal.x][animal.y] = animal

	def draw_animal_map(self, show_possible_moves = False):
		string_map = []

		for row in self.animal_map:
			map_row = []
			for animal in row:
				if animal == None:
					map_row.append('.')
				elif animal.player == MAX:
					map_row.append(animals_dict[animal.type].upper())
				else:
					map_row.append(animals_dict[animal.type])
			string_map.append(map_row)

		if show_possible_moves:
			moves_min = self.possible_moves(MIN)
			moves_max = self.possible_moves(MAX)
			for x, y, dx, dy in moves_min:
				if string_map[x+dx][y+dy] == '.':
					string_map[x+dx][y+dy] = '&'
			for x, y, dx, dy in moves_max:
				if string_map[x+dx][y+dy] == '.':
					string_map[x+dx][y+dy] = '#'

		string_map = '\n'.join([''.join(row) for row in string_map])
		print(string_map)
		print('\n')



	def possible_moves(self, player):
		moves = [] # (x, y, new_x, new_y)
		animals = self.max_animals if player == MAX else self.min_animals

		for animal in animals:
			for dx, dy in dirs:
				if self.can_move(animal, dx, dy):
					x,y = animal.x, animal.y
					moves.append((x, y, dx, dy))

		return moves

	def can_move(self, animal, dx, dy):
		animal_type, init_x, init_y, player = animal.type, animal.x, animal.y, animal.player
		if not self.is_move_valid(init_x+dx, init_y+dy):
			return False

		target = jungle_map[init_x+dx][init_y+dy]
		# own cave
		if target == UPPERCAVE and player == MAX: return False
		if target == LOWERCAVE and player == MIN: return False
		# mere
		if target == MERE and animal_type not in [RAT, TIGER, LION]: return False
		if target == MERE and animal_type in [TIGER, LION]:
			if not self.can_jump(animal, dx, dy): return False
			if self.can_jump(animal, dx, dy): return True

		target_animal = self.animal_map[init_x+dx][init_y+dy]
		if target_animal == None:
			return True

		# not free field
		target_animal_type, target_player = target_animal.type, target_animal.player
		if player == target_player: return False
		if target in [UPPERCAVE, LOWERCAVE]: return True
		if target_animal_type > animal_type and not (target_animal_type == ELEPHANT and animal_type == RAT): return False
		if target == MEADOW and jungle_map[init_x][init_y] == MERE: return False

		return True

	def is_move_valid(self, x, y):
		return 0 <= x <= 8 and 0 <= y <= 6

	def can_jump(self, animal, dx, dy):
		animal_type, x, y, player = animal.type, animal.x, animal.y, animal.player
		x, y = x+dx, y+dy
		target = jungle_map[x][y]
		while target == MERE:
			mere_animal = self.animal_map[x][y]
			if mere_animal != None and (mere_animal.type, mere_animal.player) == (RAT, 1-player):
				return False
			x, y = x+dx, y+dy
			target = jungle_map[x][y]

		target_animal = self.animal_map[x][y]
		if target_animal != None and target_animal.type > animal_type: return False
		if target_animal != None and target_animal.player == player: return False
		
		return True

	def do_move(self, move, player):
		self.moves_number += 1
		self.passive_moves += 1

		x, y, dx, dy = move
		animal_type = self.animal_map[x][y].type
		if jungle_map[x+dx][y+dy] == MERE and animal_type in [TIGER, LION]:
			jump_x, jump_y = x +dx ,y + dy 
			while jungle_map[jump_x][jump_y] == MERE:
				jump_x += dx
				jump_y += dy
			dx = jump_x - x 
			dy = jump_y - y

		target_x, target_y = x+dx, y+dy

		if self.animal_map[target_x][target_y] != None:

			self.passive_moves = 0
			devoured_animal = self.animal_map[target_x][target_y]
			self.animal_map[target_x][target_y] = None

			animals_list = self.min_animals if player == MAX else self.max_animals
			if player == MAX:
				self.min_animals.remove(devoured_animal)
			else:
				self.max_animals.remove(devoured_animal)
 	
 		self.animal_map[target_x][target_y] = self.animal_map[x][y]
 		self.animal_map[x][y] = None

 		self.animal_map[target_x][target_y].x = target_x
 		self.animal_map[target_x][target_y].y = target_y

 	def result(self):
 		if self.animal_map[0][3] != None: return MIN
 		if self.animal_map[8][3] != None: return MAX
 		if len(self.min_animals) == 0: return MAX
 		if len(self.max_animals) == 0: return MIN
 		return -1

 	def terminal(self):
 		if self.passive_moves > 50:
 			return True

 	def terminal_result(self):
 		min_values = sorted([animal.type for animal in self.min_animals], reverse=True)
 		max_values = sorted([animal.type for animal in self.max_animals], reverse=True)
 		if min_values[-1] > max_values[-1]: return MIN
 		if min_values[-1] < max_values[-1]: return MAX
 		#for i in range(min(len(min_values), len(max_values))):
 			#if min_values[i] > max_values[i]: return MIN
 			#if min_values[i] < max_values[i]: return MAX

 		min_values = sorted([abs(animal.x-0) + abs(animal.y-3) for animal in self.min_animals])
 		max_values = sorted([abs(animal.x-8) + abs(animal.y-3) for animal in self.max_animals])
 		for i in range(min(len(min_values), len(max_values))):
 			if min_values[i] < max_values[i]: return MIN
 			if min_values[i] > max_values[i]: return MAX

 		return MAX

 	def zad2_experminent(self):
 		while True:
 			#self.draw_animal_map(True)

 			if self.player == MIN:
 				m = self.greedy_move(self.player)
 			else:
 				m = self.heuristic_move(self.player)

 			#self.draw_animal_map()
 			if m != None:
 				self.do_move(m, self.player)
 			
 			self.player = 1 - self.player
 			if self.result() > -1:
 				return self.result(), self.moves_number
 			if self.terminal():
 				return self.terminal_result()

 	def random_experiment(self):
 		while True:

 			m = self.random_move(self.player)

 			if m != None:
 				self.do_move(m, self.player)
 			#self.draw_animal_map()
 			self.player = 1 - self.player
 			if self.result() > -1:
 				return self.result(), self.moves_number
 			if self.terminal():
 				return self.terminal_result()

 	def random_move(self, player):
 		moves = self.possible_moves(player)

 		if moves:
 			return random.choice(moves)
 		else:
 			return None

 	def greedy_move(self, player):
 		moves = self.possible_moves(player)

 		if not moves:
 			return None

 		new_states = [JungleGame(self) for _ in range(len(moves))]
 		for i in range(len(moves)):
 			new_states[i].do_move(moves[i], player)
 			new_states[i].player = 1-new_states[i].player

 		N = 0
 		results = [0] * len(new_states)

 		while N < 20000:
	 		for idx, state in enumerate(new_states):
	 			game_copy = JungleGame(state)
	 			result = game_copy.random_experiment()
	 			N += game_copy.moves_number - state.moves_number 
	 			if result == player:
	 				results[idx] += 1
	 			else:
	 				results[idx] -= 1

	 	#print("****** MOVE **********")
	 	#for st, m, res in zip(new_states, moves, results):
	 		#print(res)
	 		#print(m)
	 		#st.draw_animal_map()
	 		
 		move, _ = max(zip(moves, results), key=lambda x: x[1])

 		return move

 	def heuristic_move(self, player):
 		moves = self.possible_moves(player)

 		if not moves:
 			return None

 		new_states = [JungleGame(self) for _ in range(len(moves))]
 		for i in range(len(moves)):
 			new_states[i].do_move(moves[i], player)

 		results = [0] * len(new_states)
 		for idx, state in enumerate(new_states):
 			results[idx] += distance_heuristic(state, player)
	 	#print(results)
	 	if player == MAX:
 			move, _ = max(zip(moves, results), key=lambda x: x[1])
 		else:
 			move, _ = min(zip(moves, results), key=lambda x: x[1])

 		return move

def distance_heuristic(state, player):
	max_distance = sum([abs(animal.x-8) + abs(animal.y-3) for animal in state.max_animals])
	min_distance = sum([abs(animal.x-0) + abs(animal.y-3) for animal in state.min_animals])
	return min_distance-max_distance

lost = 0
win = 0
import time
copy_time = 0

for iteration in range(100):
    j = JungleGame()
    #res = j.zad2_experminent()
    res = j.zad2_experminent()
    if res == 0:
        lost += 1
    else:
        win += 1
    if iteration % 40 == 0: print(iteration)

print("Lost: %d" % lost)
print("Win: %d" % win)
print(copy_time)