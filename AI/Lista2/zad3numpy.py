import numpy as np
from collections import deque
import heapq

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
	if sokoban_map[box_position] == 'G':
		return False
	moves = np.array([(1,0), (-1,0), (0,1), (0,-1)])
	surr_coords = np.array(box_position) + moves  

	return not ((sokoban_map[tuple(surr_coords[0])] in 'G.' and sokoban_map[tuple(surr_coords[1])] in 'G.') or \
				(sokoban_map[tuple(surr_coords[2])] in 'G.' and sokoban_map[tuple(surr_coords[3])] in 'G.'))


def possible_new_states(sokoban_map, player_position, box_positions):
	moves = [(1,0), (-1,0), (0,1), (0,-1)]
	player_position = tuple(player_position)
	box_positions = (box_positions).tolist()
	box_positions = [tuple(b) for b in box_positions]
	new_states = []
	checked_positions = {tuple(player_position):""}
	q = deque([player_position])
	while q:
		player_position = q.pop()

		for move in moves:
			new_player_pos = (player_position[0] + move[0], player_position[1] + move[1])
			new_box_pos = (player_position[0] + 2*move[0], player_position[1] + 2*move[1])

			if sokoban_map[new_player_pos] in 'G.' and not (new_player_pos in box_positions) \
					and not new_player_pos in checked_positions:
				checked_positions[new_player_pos] = checked_positions[player_position] + name_of_move(player_position, new_player_pos)
				q.appendleft(new_player_pos)
			elif sokoban_map[new_player_pos] in 'G.' and (new_player_pos in box_positions) and\
					sokoban_map[new_box_pos] in 'G.' and not (new_box_pos in box_positions) \
					and not bad_box_position(sokoban_map, new_box_pos):	

				new_box_positions = [pos for pos in box_positions if pos != new_player_pos]
				new_box_positions.append(new_box_pos)

				path = checked_positions[player_position] + name_of_move(player_position, new_player_pos)
				if check_if_won(sokoban_map, new_box_positions):
					return (-1, path)

				new_states.append((new_player_pos, new_box_positions, path))

	return new_states

def Aheuristic(sokoban_map, player_position, box_positions):
	result = 0
	box_positions = np.array(box_positions)
	for idx in range(box_positions.shape[0]):
		result += np.amin(np.abs(target_positions - box_positions[idx]).sum(axis=1))
	return result

states_checked = 0
def find_moves_A(sokoban_map, init_player_position, init_box_positions):
	q = [] 
	cost = Aheuristic(sokoban_map, init_player_position, init_box_positions)
	heapq.heappush(q, (cost, init_player_position, init_box_positions))
	checked_states = { (tuple(init_player_position), tuple(map(tuple, init_box_positions))):"" } # to reconstruct path
	states_checked = 0
	while len(q) > 0:
		states_checked += 1

		_, player_pos, boxes_pos = heapq.heappop(q)
		possible_states = possible_new_states(sokoban_map, player_pos, boxes_pos)

		hashable_prev_state = (tuple(player_pos), tuple(map(tuple, init_box_positions)))
		if len(possible_states) > 0 and possible_states[0] == -1:
			print(states_checked)
			return checked_states[hashable_prev_state] + possible_states[1]

		for state in possible_states:
			hashable_state = (tuple(state[0]), tuple(map(tuple, init_box_positions)))
			if not (hashable_state in checked_states):
				checked_states[hashable_state] = checked_states[hashable_prev_state] + state[2]#name_of_move(player_pos, state[0])
				cost = Aheuristic(sokoban_map, state[0], state[1])
				heapq.heappush(q, (cost , state[0], state[1])) 
				
f = open("zad_input.txt", 'r')
f_out = open("zad_output.txt", 'w')

sokoban_map = []
player_position = None
box_positions = []
target_positions = []
for r_idx, row in enumerate(f):
	map_row = []
	for c_idx, elem in enumerate(row):
		if elem == 'G':
			target_positions.append((r_idx, c_idx))
		if elem in "WG.":
			map_row.append(elem)
		if elem in "KB":
			map_row.append('.')
		if elem in "+*":
			map_row.append('G')
			target_positions.append((r_idx, c_idx))
		if elem in 'K+':
			player_position = (r_idx, c_idx)
		if elem in "B*":
			box_positions.append((r_idx, c_idx))
	sokoban_map.append(map_row)

target_positions = np.array(target_positions)
output = find_moves_A(np.array(sokoban_map), np.array(player_position), np.array(box_positions))
#print(output)
f_out.write(output)