from math import sqrt, log
import random
import sys
import itertools
#import numpy as np
from collections import defaultdict as dd

BOK = 30
SX = -100
SY = 0
M = 8

def initial_board():
	B = [ [-1] * M for i in range(M)]
	B[3][3] = 1
	B[4][4] = 1
	B[3][4] = 0
	B[4][3] = 0
	return B

moves_dict = {}
	
class Board:
	dirs  = [ (0,1), (1,0), (-1,0), (0,-1), (1,1), (-1,-1), (1,-1), (-1,1) ]
	
	
	def __init__(self, starting_board=None): 
		self.player = None
		if starting_board == None:
			self.board = initial_board()
			self.fields = set()
			self.move_list = []
			self.dics_num = 4
			self.history = []
			self.new_fields(3,3)
			self.new_fields(4,4)
			self.new_fields(3,4)
			self.new_fields(4,3)
		else:
			self.board = [list(row) for row in starting_board.board]
			self.dics_num = starting_board.dics_num
			self.fields = set(starting_board.fields)
			self.move_list = list(starting_board.move_list)
			self.history = []
			self.player =starting_board.player
		

	def new_fields(self, x,y):
		for d in self.dirs:
			x0, y0 = d
			neighbour = self.get(x+x0,y+y0)
			if neighbour != None and neighbour < 0:
				self.fields.add( (x+x0,y+y0) )

	def draw(self):
		for i in range(M):
			res = []
			for j in range(M):
				b = self.board[i][j]
				if b == -1:
					res.append('.')
				elif b == 1:
					res.append('#')
				else:
					res.append('o')
			print ''.join(res) 
		print            
		
	
	def show(self):
		for i in range(M):
			for j in range(M):
				kwadrat(j, i, 'green')
				
		for i in range(M):
			for j in range(M):                
				if self.board[i][j] == 1:
					kolko(j, i, 'black')
				if self.board[i][j] == 0:
					kolko(j, i, 'white')
								   
	def moves(self, player):
		res = []

		for (x,y) in self.fields:
			if any( self.can_beat(x,y, direction, player) for direction in Board.dirs):
				res.append( (x,y) )
		
		if not res:
			return [None]

		return res

	def one_move(self, player):
		fields=list(self.fields)
		random.shuffle(fields)
		for (x,y) in fields:
			if any( self.can_beat(x,y, direction, player) for direction in Board.dirs):
				return (x,y)
		return None
	
	def can_beat(self, x,y, d, player):
		dx,dy = d
		x += dx
		y += dy
		cnt = 0
		while self.get(x,y) == 1-player:
			x += dx
			y += dy
			cnt += 1
		return cnt > 0 and self.get(x,y) == player
	
	def get(self, x,y):
		if 0 <= x < M and 0 <=y < M:
			return self.board[x][y]
		return None
						
	def do_move(self, move, player):
		#self.history.append([x[:] for x in self.board])
		self.move_list.append(move)

		if move == None:
			return
		x,y = move
		x0,y0 = move
		self.board[x][y] = player
		self.new_fields(x,y)
		self.fields -= set([move])
		self.dics_num += 1
		for dx,dy in self.dirs:
			x,y = x0,y0
			to_beat = []
			x += dx
			y += dy
			while self.get(x,y) == 1-player:
			  to_beat.append( (x,y) )
			  x += dx
			  y += dy
			if self.get(x,y) == player:              
				for (nx,ny) in to_beat:
					self.board[nx][ny] = player
													 
	def result(self):
		res = 0
		for y in range(M):
			for x in range(M):
				b = self.board[x][y]                
				if b == 0:
					res -= 1
				elif b == 1:
					res += 1
		return res
				
	def terminal(self):
		if not self.fields:
			return True
		if len(self.move_list) < 2:
			return False
		return self.move_list[-1] == self.move_list[-2] == None 

	def random_move(self, player):
		ms = self.moves(player)
		
		if ms:
			return random.choice(ms)
		return None

	def strategic_move(self, player):
		ms = self.moves(player)

		if ms[0] == None:
			return None

		minmax_instance = AlfaBeta()
		if len(self.move_list) < 40:
			return minmax_instance.decision_max(self, player, 2)
		elif len(self.move_list) < 55:
			return minmax_instance.decision_max(self, player, 2)
		else:
			end_game = True
			return minmax_instance.decision_max(self, player, 2)

end_game = False
class AlfaBeta:

	def decision_max(self, starting_B, player, max_depth=2):
		self.max_depth = max_depth
		moves = starting_B.moves(player)
		if moves[0] == None:
			return moves

		states = []
		for m in moves:
			B = Board(starting_B)
			B.do_move(m, player)
			states.append(B)

		alfa = -9999999
		beta = 9999999
		for m, s in zip(moves, states):
			value = self.min_value(s, 1-player, 0, alfa, beta)
			if value >= alfa:
				best_move = m
			alfa = max(alfa, value)

		return best_move


	def min_value(self, B, player, depth, alpha, beta):

		if depth==self.max_depth or B.terminal():
			return heuristic_value(B, player)
		moves = B.moves(player)
		if moves[0] == None:
			return heuristic_value(B, player)


		value = 999999
		for m in moves:
			B_result = Board(B)
			B_result.do_move(m, player)
			
			value = min(value,self.max_value(B_result, 1-player, depth+1, alpha, beta))
			if value <= alpha:
				return value
			beta = min(beta, value)

		return value
		

	def max_value(self, B, player, depth, alpha, beta):

		if depth==self.max_depth or B.terminal():
			return heuristic_value(B, player)

		moves = B.moves(player)
		if moves[0] == None:
			return heuristic_value(B, player)
		
		value = -999999
		for m in moves:
			B_result = Board(B)
			B_result.do_move(m, player)
			
			value = max(value,self.min_value(B_result, 1-player, depth+1, alpha, beta))
			if value >= beta:
				return value
			alpha = max(alpha, value)

		return value

###########################################################
# Heuristics

def front_tiles_heuristic(B):
	res = 0
	checked = set()
	min_front_tiles = 0
	max_front_tiles = 0
	for x, y in B.fields:
		for d in B.dirs:
			dx, dy = d
			if B.get(x+dx, y+dy) == 1 and not (x+dx, y+dy) in checked:
				max_front_tiles += 1
				checked.add((x+dx, y+dy))
				break
			if B.get(x+dx, y+dy) == 0 and not (x+dx, y+dy) in checked:
				min_front_tiles += 1
				checked.add((x+dx, y+dy))
				break
	if max_front_tiles + min_front_tiles == 0: return 0

	return 100.0*float(min_front_tiles - max_front_tiles)/(max_front_tiles + min_front_tiles )

def coin_parity_heuristic(B):
	res = 0
	total = 0
	for y in range(M):
		for x in range(M):
			b = B.board[x][y]                
			if b == 0:
				res -= 1
				total += 1
			elif b == 1:
				res += 1
				total += 1

	return 100.0*float(res)/total

def corners_heuristic(B):
	res = 0
	total = 0
	for cor_x, cor_y in [(0,0), (0,7), (7,0), (7,7)]:
		if B.board[cor_x][cor_y] == 0:
			res -= 1
		elif B.board[cor_x][cor_y] == 1:
			res += 1

	return 25*res

def corners_closneness_heuristic(B):
	corners = [(0,0), (0,7), (7,0), (7,7)]
	nears = [[(0,1), (1,0), (1,1)],
		[(1,7), (0,6), (1,6)],
		[(7,1), (6,0), (6,1)],
		[(7,6), (6,6), (6,7)]]
	res = 0
	for (cor_x, cor_y), near in zip(corners, nears):
		if B.board[cor_x][cor_y] != -1: continue
		for near_x, near_y in near:
			if B.board[near_x][near_y] == 0:
				res += 1
			elif B.board[near_x][near_y] == 1:
				res -= 1

	return 8*res


def mobility_heuristic(B):
	max_moves = number_of_moves(B, 1)
	min_moves = number_of_moves(B, 0)
	if max_moves + min_moves == 0:
		mobility = 0
	else:
		mobility = 100.0*float((max_moves - min_moves))/(max_moves + min_moves)
	return mobility

def number_of_moves(B, player):
	moves = B.moves(player)
	if moves[0] == None:
		return 0 
	return len(moves)

def fields_values_heuristic(B):
	board = B.board
	
	res = 0
	for row in range(M):
		for col in range(M):
			if board[row][col] == 1:
				res += heuristics[B.dics_num][row][col]
			elif board[row][col] == 0:
				res -= heuristics[B.dics_num][row][col]
	return res

def heuristic_value(B, player):
	## mobility
	mobility = mobility_heuristic(B)
	## corners
	cors = corners_heuristic(B)
	## corner closeness
	closeness = corners_closneness_heuristic(B)
	## stability
	#stability = front_tiles_heuristic(B)

	result = 0
	if end_game:
		## coin parity
		coin_p = coin_parity_heuristic(B)
		result += 20*coin_p
	else:
		positions = fields_values_heuristic(B)
		result += 100*positions
	#return 78.922*mobility + 10*coin_p + 801.724*cors + 74.396*stability + 10*res + l
	return result + 200*cors  + 150 * closeness + 20 * mobility

#####################################
#Heuristic map
random.seed(500)
import pickle
pkl_file = open('heuristics.pkl', 'rb')
heuristics = pickle.load(pkl_file)
for h in heuristics:
	h_max = sum([sum(row) for row in h])
	if h_max == 0: h_max = 1.0
	for r in range(8):
		for c in range(8):
			h[r][c] /= float(h_max)

#####################################
class Monte_Carlo_Node(object):

	def __init__(self):
		self.plays = 0
		self.wins = 0
		self.children = {} # child_nodes, key = move
		self.moves = None # possible moves
		self.depth=0
		

class Monte_Carlo(object): 

	def __init__(self):
		self.C = 1.0

	def get_move(self, board, enemy_move):
		if enemy_move and enemy_move in self.root.children:
			self.root = self.root.children[enemy_move]
		else:
			self.root = Monte_Carlo_Node()

		self.root_player = board.player

		possible_moves = board.moves(board.player)
		for i in range(len(self.root.children)):
			if possible_moves[i] not in self.root.children: continue
			child = self.root.children[possible_moves[i]]
			print(child.wins, child.plays, possible_moves[i])

		if len(possible_moves) == 0:
			return None
		elif len(possible_moves) == 1:
			return possible_moves[0]

		playouts = 0
		t0 = time.time()
		while time.time() - t0 < 0.5:
			iters = 100
			playouts +=  iters
			for _ in range(100):
				self.run_simulation(board)

		print('playouts', playouts)

		root_node = self.root
		moves = [(root_node.children[m].plays, root_node.children[m].wins, m) 
			for m in possible_moves if m in root_node.children]

		print(moves)
		_, _,  best_move = max(moves, key=lambda m: m[0])
		#_, _,  best_move = max(moves, key=lambda m: float(m[1])/m[0])
		self.root = self.root.children[best_move]
		
		return best_move

	def run_simulation(self, board):
		board_copy = Board(board)
		visited = set()
		visited.add(self.root)

		tree_node = self.root
		expanding = True
		while True:
			if board_copy.terminal():
				winner = 1 if board_copy.result() > 0 else 0
				break

			if expanding:
				possible_moves = board_copy.moves(board_copy.player)

			if len(tree_node.children) == len(possible_moves):
				best_move = None
				best_value = 0.0
				for m in possible_moves:

					child_node = tree_node.children[m]
					if board_copy.player == self.root_player:
						win_num = child_node.wins
					else:
						win_num = child_node.plays - child_node.wins

					value = float(win_num)/child_node.plays
					value += self.C*sqrt(log(float(tree_node.plays))/child_node.plays)
					
					if value > best_value:
						best_value = value
						best_move = m
				move = best_move
			elif expanding:
				moves = [m for m in possible_moves if m not in tree_node.children]
				move = random.choice(moves)
				tree_node.children[move] = Monte_Carlo_Node()
				expanding = False
			else:
				winner = random_experiment(board_copy)
				break
				
			board_copy.do_move(move, board_copy.player)
			board_copy.player = 1 - board_copy.player
			
			visited.add(tree_node.children[move])
			tree_node = tree_node.children[move]
			
		for node in visited:
			node.plays += 1
			if winner == self.root_player:
				node.wins += 1

def random_experiment(board):
	m = None
	while True:

		m = board.one_move(board.player)
		board.do_move(m, board.player)

		board.player = 1-board.player
		if board.terminal():
			break

	return 1 if board.result() > 0 else 0

def monte_carlo_experiment():
	global total_random
	global total_move
	B = Board()
	MC = Monte_Carlo()
	B.player = 0
	m = None
	while True:
		B.draw()
		if B.player == 0:
			m = MC.get_move(B, m)
		else:
			t0 = time.time()
			m = B.strategic_move(B.player)
			print('minmax time: ', time.time()-t0)
			#m = self.random_move(player)

		B.do_move(m, B.player)

		B.player = 1-B.player
		if B.terminal():
			break

	return B.result()

import time 
wins = 0
lost = 0
for iteration in range(10):
	B = Board()
	#res = B.experiment()
	res = monte_carlo_experiment()
	if res <= 0:
		lost += 1
	else:
		wins += 1
	if iteration % 1 == 0: print(iteration)

print('Wins: %d' % wins)
print('Lost: %d' % lost)
print('Win rate : %d' % (100.0*wins/(wins+lost)))
