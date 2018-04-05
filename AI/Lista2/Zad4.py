import numpy as np
from collections import deque

dirs = [(np.array((1,0)), 'D'), 
 	(np.array((-1,0)), 'U'), 
  	(np.array((0,1)), 'R'), 
   	(np.array((0,-1)), 'L')]

def draw_map(commando_map, commando_destinations_mask, positions):
	to_print = commando_map.copy()
	to_print[positions[:,0], positions[:,1]] += 1
	to_print[commando_destinations_mask == 0] += 2
	print('*************')
	for row in to_print:
		r = ""
		for col in row:
			if col == 0:
				r +='#'
			elif col == 1:
				r += ' '
			elif col == 2:
				r += 'S'
			elif col == 3:
				r += 'G'
			elif col == 4:
				r += 'B'
		print(r)

def reconstruct_path(predecessors, ending_pos):
	path = ""
	ending_pos_tup = tuple(ending_pos)
	while predecessors[ending_pos_tup] != None:
		path = predecessors[ending_pos_tup][1] + path
		ending_pos = predecessors[ending_pos_tup][0]
		ending_pos_tup = tuple(ending_pos)

	return path

def do_move_no_numpy(old_positions, move, commando_map):
	new_positions = set()
	for pos in old_positions:
		new_pos = (pos[0] + move[0], pos[1] + move[1])
		if commando_map[new_pos[0], new_pos[1]] == 1:
			new_positions.add(new_pos)
		else:
			new_positions.add(pos)
	return new_positions

def do_move(old_positions, move, commando_map):
	new_positions = old_positions + move

	updated = commando_map[new_positions[:,0], new_positions[:,1]][:, None]
	not_updated = (1 - updated).astype(int)

	new_positions = np.unique(updated * new_positions + not_updated * old_positions, axis=0)
	return new_positions

def find_possible(commando_map, commando_destinations_mask, positions):
	new_states = []

	for move, direction in dirs:
		new_positions = do_move_no_numpy(positions, move, commando_map)
		new_states.append( (new_positions, direction) )

		if target_num >= len(new_positions) and solved(new_positions, commando_destinations_mask):
			return ('found', direction)

	return new_states


def solved(positions, commando_destinations_mask):
	for pos in positions:
		if commando_destinations_mask[pos[0], pos[1]] == 1:
			return False
	return True

import time 

def find_path_bfs(commando_map, commando_destinations_mask, commando_positions):
	commando_positions = list(map(tuple, commando_positions))
	q = deque([commando_positions])
	visited = { tuple(commando_positions) : None }
	while q:
		positions = q.pop()
		
		possible_states = find_possible(commando_map, commando_destinations_mask, positions)

		if len(possible_states) > 0 and possible_states[0] == 'found':
			last_direction = possible_states[1]
			return reconstruct_path(visited, positions) + last_direction

		for pos_arr, direction in possible_states:
			tup_pos_arr = tuple(pos_arr)
			if tup_pos_arr not in visited:
				visited[tup_pos_arr] = (positions, direction)
				q.appendleft(pos_arr)

def random_moves_greedy(positions, commando_map):
	moves_num = 35
	path = ""
	while moves_num > 0:
		best_move = None
		best_dir = None
		best_positions_num = len(positions)
		for move, direction in dirs:
			new_positions = do_move(positions, move, commando_map)
			if len(new_positions) < best_positions_num:
				best_move, best_dir = move, direction
				best_positions_num = len(new_positions)

		if best_dir == None:
			move, direction = dirs[np.random.randint(0, len(dirs))]
			move_len = int(min(max(1, np.random.normal(3, 2)),10))
			for i in range(move_len):
				positions = do_move(positions, move, commando_map)
				path += direction
				moves_num -= 1
		else:
			positions = do_move(positions, best_move, commando_map)
			path += best_dir
			moves_num -= 1

	return positions, path

def random_moves(positions, commando_map, moves_num):
	path =""
	while len(path) < moves_num:
		move, direction = dirs[np.random.randint(0, len(dirs))]
		move_len = int(min(max(1, np.random.normal(3, 2)),10))
		for i in range(move_len):
			positions = do_move(positions, move, commando_map)
			path += direction

	return positions, path

f_in = open('zad_input.txt', 'r')
f_out = open('zad_output.txt', 'w')

commando_map = []
commando_starts = []
commando_destinations_mask = []

for line_number ,line in enumerate(f_in):
	row = []
	for letter_number, letter in enumerate(line):
		if letter == '#':
			row.append(0)
		elif letter in ' S':
			row.append(1)
		elif letter in 'BG':
			row.append(2)
		if letter in 'SB':
			commando_starts.append((line_number, letter_number))
	commando_map.append([1 if letter > 0 else 0 for letter in row])
	commando_destinations_mask.append([0 if letter > 1 else 1 for letter in row])

commando_starts = np.array(commando_starts)
commando_map = np.array(commando_map)
commando_destinations_mask = np.array(commando_destinations_mask)
target_num = commando_destinations_mask[commando_destinations_mask == 0].shape[0]

commando_starts_init = commando_starts.copy()
path1 = ""
path2 = ""
while commando_starts.shape[0] > 2:
	commando_starts = commando_starts_init.copy()
	commando_starts, path1 = random_moves_greedy(commando_starts, commando_map)
	commando_starts_greedy = commando_starts.copy()
	i = 0
	while commando_starts.shape[0] > 2 and i < 40:  
		commando_starts = commando_starts_greedy.copy()
		commando_starts, path2 = random_moves(commando_starts, commando_map, 65)
		i += 1

path = path1 + path2 + find_path_bfs(commando_map, commando_destinations_mask, commando_starts)
f_out.write(path)