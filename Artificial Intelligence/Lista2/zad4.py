from collections import deque
import random

dirs = [((1,0), 'D'), 
 	((-1,0), 'U'), 
  	((0,1), 'R'), 
   	((0,-1), 'L')]

def draw_map(commando_map, commando_destinations, positions):
	to_print = [[c for c in r] for r in commando_map]
	for x,y in positions:
		to_print[x][y] += 1
	for i in range(len(commando_destinations)):
		for j in range(len(commando_destinations[i])):
			if commando_destinations[i][j] == 1:
				to_print[i][j] += 2
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

def do_move(old_positions, move, commando_map):
	new_positions = set()
	for pos in old_positions:
		new_pos = (pos[0] + move[0], pos[1] + move[1])
		if commando_map[new_pos[0]][new_pos[1]] == 1:
			new_positions.add(new_pos)
		else:
			new_positions.add(pos)
	return new_positions

def find_possible(commando_map, positions):
	new_states = []

	for move, direction in dirs:
		new_positions = do_move(positions, move, commando_map)
		new_states.append( (new_positions, direction) )

	return new_states


def solved(positions, commando_destinations):
	for pos in positions:
		if commando_destinations[pos[0]][pos[1]] == 0:
			return False
	return True
	
def find_path_bfs(commando_map, commando_destinations, commando_positions):
	q = deque([commando_positions])
	visited = { tuple(commando_positions) : None }
	while q:
		positions = q.pop()
		
		possible_states = find_possible(commando_map, positions)

		solved_states = [len(x) <= target_num and solved(x, commando_destinations) 
			for x, _ in possible_states]
		
		if any(solved_states):
			_, last_direction = dirs[solved_states.index(True)]
			draw_map(commando_map, commando_destinations, positions)
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
			dir_idx = random.randint(0, len(dirs)-1)
			move, direction = dirs[dir_idx]
			move_len = int(min(max(1, random.normalvariate(3, 2)),10))
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
	path=""
	while len(path) < moves_num:
		move, direction = dirs[random.randint(0, len(dirs)-1)]
		move_len = int(min(max(1, random.normalvariate(3, 2)),10))
		for i in range(move_len):
			positions = do_move(positions, move, commando_map)
			path += direction

	return positions, path

f_in = open('zad_input.txt', 'r')
f_out = open('zad_output.txt', 'w')

commando_map = []
commando_starts = []
commando_destinations = []

for line_number, line in enumerate(f_in):
	row = []
	for letter_num, letter in enumerate(line):
		if letter == '#':
			row.append(0)
		elif letter in ' S':
			row.append(1)
		elif letter in 'BG':
			row.append(2)
		if letter in 'SB':
			commando_starts.append((line_number, letter_num))
	commando_map.append([1 if f > 0 else 0 for f in row])
	commando_destinations.append([1 if f > 1 else 0 for f in row])

target_num = sum([sum(r) for r in commando_destinations])

commando_starts_init = [tup for tup in commando_starts]
path1 = ""
path2 = ""
while len(commando_starts) > 2:
	commando_starts = [tup for tup in commando_starts_init]
	commando_starts, path1 = random_moves_greedy(commando_starts, commando_map)
	commando_starts_reduced = [tup for tup in commando_starts]
	tries = 0
	while len(commando_starts) > 2 and tries < 40:
		tries += 1
		commando_starts = [tup for tup in commando_starts_reduced]
		commando_starts, path2 = random_moves(commando_starts,
            commando_map,
            65)

path = path1 + path2
path += find_path_bfs(commando_map,
    commando_destinations, 
    commando_starts)

f_out.write(path)