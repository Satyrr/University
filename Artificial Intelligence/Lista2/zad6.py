import numpy as np
from collections import deque
import heapq
import time
from itertools import count

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

def do_move(old_positions, move, commando_map):
	new_positions = old_positions + move

	updated = commando_map[new_positions[:,0], new_positions[:,1]][:, None]
	if updated.sum() == 0:
		return np.array([])
	not_updated = (1 - updated).astype(int)

	new_positions = np.unique(updated * new_positions + not_updated * old_positions, axis=0)
	return new_positions

def find_possible(commando_map, commando_destinations_mask, positions):
	new_states = []

	for move, direction in dirs:
		new_positions = do_move(positions, move, commando_map)
		if new_positions.shape[0] == 0: continue
		new_states.append( (new_positions, direction) )

		if target_num >= new_positions.shape[0] and \
			commando_destinations_mask[new_positions[:,0], new_positions[:,1]].sum() == 0:
			return ('found', direction)

	return new_states

def heuristic(commando_positions):
	return 20*distances_mat[commando_positions[:,0], commando_positions[:,1]].max()


def find_path_bfs(commando_map, commando_destinations_mask, commando_positions):
	q = []
	heapq.heappush(q, (heuristic(commando_positions), tuple(map(tuple, commando_positions))))
	visited = { tuple(map(tuple, commando_positions)) : "" }
	while q:
		_, positions = heapq.heappop(q)
		positions = np.array(positions)

		possible_states = find_possible(commando_map, commando_destinations_mask, positions)

		if len(possible_states) > 0 and possible_states[0] == 'found':
			#print('found in:', iterations)
			last_direction = possible_states[1]
			return visited[tuple(map(tuple, positions))] + last_direction

		path_to_current_position = visited[tuple(map(tuple, positions))]
		for pos_arr, direction in possible_states:
			tup_pos_arr = tuple(map(tuple, pos_arr))
			if tup_pos_arr not in visited:
				visited[tup_pos_arr] = path_to_current_position + direction
				heapq.heappush(q, (heuristic(pos_arr) + len(visited[tup_pos_arr]), tuple(map(tuple, pos_arr))))

def min_distance_bfs():
	distances_mat = np.ones(commando_map.shape) * (-1)
	
	for destin in commando_destinations:
		visited = { tuple(destin) : None }
		q = deque([(destin, 0)])
		while q:
			v, cost = q.pop()

			if distances_mat[v[0], v[1]] > cost or distances_mat[v[0], v[1]] == -1:
				distances_mat[v[0], v[1]] = cost

			for d in dirs:
				new_v = v + d[0]
				if commando_map[new_v[0], new_v[1]] == 0: continue
				if tuple(new_v) not in visited:
					q.appendleft((new_v, cost+1))
					visited[tuple(new_v)] = None
	return distances_mat


f_in = open('zad_input.txt', 'r')
f_out = open('zad_output.txt', 'w')

commando_map = []
commando_starts = []
commando_destinations_mask = []
commando_destinations = []

for line_number ,line in enumerate(f_in):
	row = []
	for letter_number, letter in enumerate(line):
		if letter == '#':
			row.append(0)
		elif letter in ' S':
			row.append(1)
		elif letter in 'BG':
			row.append(2)
			commando_destinations.append((line_number, letter_number))
		if letter in 'SB':
			commando_starts.append((line_number, letter_number))
	commando_map.append([1 if letter > 0 else 0 for letter in row])
	commando_destinations_mask.append([0 if letter > 1 else 1 for letter in row])

commando_starts = np.array(commando_starts)
commando_map = np.array(commando_map)
commando_destinations_mask = np.array(commando_destinations_mask)
commando_destinations = np.array(commando_destinations)
target_num = commando_destinations_mask[commando_destinations_mask == 0].shape[0]
distances_mat = min_distance_bfs()

path = find_path_bfs(commando_map, commando_destinations_mask, commando_starts)
f_out.write(path)
print(len(path))