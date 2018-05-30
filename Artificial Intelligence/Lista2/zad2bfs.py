import numpy as np
from collections import deque

def check_if_won(sokoban_map, box_positions):
	for box in box_positions:
		if sokoban_map[box] != 'G':
			return False

	return True

def name_of_move(prev_pos, next_post):
	move = (next_post[0] - prev_pos[0], next_post[1] - prev_pos[1])
	if move[0] == 1: return 'D'
	if move[0] == -1: return 'U'
	if move[1] == 1: return 'R'
	if move[1] == -1: return 'L'

def bad_box_position(sokoban_map, box_position):
	moves = np.array([(1,0), (-1,0), (0,1),  (0,-1)])
	if sokoban_map[box_position] == 'G':
		return False

	surr_coords = np.array(box_position) + moves  
	surr_coords = [tuple(c) for c in list(surr_coords)]
	return not ((sokoban_map[surr_coords[0]] in 'G.' and sokoban_map[surr_coords[1]] in 'G.') or \
				(sokoban_map[surr_coords[2]] in 'G.' and sokoban_map[surr_coords[3]] in 'G.'))

def possible_new_states(sokoban_map, player_pos, box_positions):
	moves = [(1,0), (-1,0), (0,1), (0,-1)]
	new_states = []
	for move in moves:
		new_player_pos = (player_pos[0] + move[0], player_pos[1] + move[1])
		new_box_pos = (player_pos[0] + 2*move[0], player_pos[1] + 2*move[1])

		if sokoban_map[new_player_pos] in 'G.'  and not \
			(new_player_pos in box_positions):
			new_states.append((new_player_pos, box_positions))
		elif sokoban_map[new_player_pos] in 'G.' and \
				sokoban_map[new_box_pos] in 'G.' and not \
				new_box_pos in box_positions and not \
				bad_box_position(sokoban_map, new_box_pos):	

			new_box_positions = [pos 
				for pos in box_positions if pos != new_player_pos]
			new_box_positions.append(new_box_pos)

			if check_if_won(sokoban_map, new_box_positions):
				return (-1, new_player_pos)

			new_states.append((new_player_pos, new_box_positions))
	return new_states

states_checked = 0
def find_moves_bfs(sokoban_map, init_player_position, init_box_positions):
	q = deque([(init_player_position, init_box_positions)])
	# to reconstruct path
	paths = { tuple((init_player_position, tuple(init_box_positions))):"" } 
	while len(q) > 0:

		player_pos, boxes_pos = q.popleft()
		possible_states = possible_new_states(sokoban_map, player_pos, boxes_pos)

		string_prev_state = tuple((player_pos, tuple(boxes_pos)))
		if possible_states[0] == -1:
			last_move = name_of_move(player_pos, possible_states[1])
			return paths[string_prev_state] + last_move
			
		for state in possible_states:
			string_state = tuple((state[0], tuple(state[1])))
			if not (string_state in paths):
				current_move = name_of_move(player_pos, state[0])
				paths[string_state] = paths[string_prev_state] + current_move
				q.append(state) 
				
f = open("zad_input.txt", 'r')
f_out = open("zad_output.txt", 'w')

sokoban_map = []
player_position = None
box_positions = []
for r_idx, row in enumerate(f):
	map_row = []
	for c_idx, elem in enumerate(row):
		if elem in "WG.":
			map_row.append(elem)
		if elem in "KB":
			map_row.append('.')
		if elem in "+*":
			map_row.append('G')
		if elem in 'K+':
			player_position = (r_idx, c_idx)
		if elem in "B*":
			box_positions.append((r_idx, c_idx))
	sokoban_map.append(map_row)

output = find_moves_bfs(np.array(sokoban_map), player_position, box_positions)
f_out.write(output)
#print(output)