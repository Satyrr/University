from collections import deque
import numpy as np

def revise_rows(possible_positions, image):
	revised = False

	for r in range(row_num):
		pos_mat = possible_positions[r]
		image_row = image[r, :]

		result = pos_mat + image_row
		result = np.all(result != 1, axis=1)

		revised |= np.any(result == False)

		possible_positions[r] = possible_positions[r][result,:]
	return revised

def revise_cols(possible_positions, image):
	revised = False
	for c in range(col_num):
		pos_mat = possible_positions[c]
		image_col = image[:, c]

		result = pos_mat + image_col
		result = np.all(result != 1, axis=1)

		revised |= np.any(result == False)

		possible_positions[c] = possible_positions[c][result, :]
	return revised

def fix_by_rows(possible_positions, image):
	for r in range(row_num):
		pos_mat = possible_positions[r]
		result = pos_mat.sum(axis=0)
		image[r, result==len(possible_positions[r])] = 1
		image[r, result==0] = 0

def fix_by_cols(possible_positions, image):
	for c in range(col_num):
		pos_mat = possible_positions[c]
		result = pos_mat.sum(axis=0)
		image[result==len(possible_positions[c]), c] = 1
		image[result==0, c] = 0	

def revise(row_possible_positions, col_possible_positions, image):
	revised = True
	while revised:
		revised = False
		image_sum = image.sum()
		revised |= revise_rows(row_possible_positions, image)
		fix_by_rows(row_possible_positions, image)
		revised |= revise_cols(col_possible_positions, image)
		fix_by_cols(col_possible_positions, image)
		revised |= image_sum < image.sum()

def solve(row_descriptions_list, col_descriptions_list):

	row_possible_positions = [np.array(binary_descripts(block_domains(col_num, row_description), row_description, col_num)) \
		for row_description in row_descriptions_list]

	col_possible_positions = [np.array(binary_descripts(block_domains(row_num, col_description), col_description, row_num)) \
		for col_description in col_descriptions_list]

	image = np.ones((row_num, col_num)) * (-1)
	

	#revise(row_possible_positions, col_possible_positions, image)
	_, image = backtrack(row_possible_positions, np.array([False] * len(row_possible_positions)), \
		col_possible_positions, np.array([False] * len(col_possible_positions)), image)

	print_image(image)
	draw_image(image)

import time
total_time = 0
def backtrack(row_variables, assigned_rows, col_variables, assigned_cols, image, depth=0):

	if np.all(assigned_rows) and np.all(assigned_cols):
		print_image(image)
		return True, image
	
	var_type, var_idx = choose_variable(row_variables, assigned_rows, col_variables, assigned_cols)
	if var_type == 'r':
		var = row_variables[var_idx]
		assigned_rows[var_idx] = True
	else:
		var = col_variables[var_idx]
		assigned_cols[var_idx] = True

	global total_time
	t0 = time.time()
	sorted_values = sort_values(row_variables, col_variables, var, var_idx, var_type, image)#reversed(range(len(var)))
	total_time += time.time() - t0
	for value_idx in sorted_values:
		image_copy = image.copy()
		row_variables_copy = [v.copy() for v in row_variables]
		col_variables_copy = [v.copy() for v in col_variables]


		assign_value(var[value_idx, :], var_idx, var_type, image_copy)
		revise(row_variables_copy, col_variables_copy, image_copy)

		result, result_img = backtrack(row_variables_copy, assigned_rows, \
			col_variables_copy, assigned_cols, image_copy, depth+1)
		if result == True:
			return True, result_img

	if var_type == 'r':
		assigned_rows[var_idx] = False
	else:
		assigned_cols[var_idx] = False

	return False, None

def assign_value(vector, vector_idx, vector_type, image):
	if vector_type == 'r':
		image[vector_idx, :] = vector
	else:
		image[:, vector_idx] = vector

def choose_variable(row_variables, assigned_rows, col_variables, assigned_cols):
	
	best_var = '', 999999, -1
	for i in range(len(row_variables)):
		if assigned_rows[i] == False and best_var[1] > row_variables[i].shape[0]:
			best_var = 'r', row_variables[i].shape[0], i


	for i in range(len(col_variables)):
		if assigned_cols[i] == False and best_var[1] > col_variables[i].shape[0]:
			best_var = 'c', col_variables[i].shape[0], i

	return best_var[0], best_var[2]


def update_assigned(row_variables, assigned_rows, col_variables, assigned_cols):
	for i, row in enumerate(row_variables):
		if row.shape[0] == 1:
			assigned_rows[i] = True
	for i, col in enumerate(col_variables):
		if row.shape[0] == 1:
			assigned_cols[i] = True

def sort_values(row_poss, col_poss, values, vector_idx, vector_type, image):
	indicies = []
	img_cpy = image.copy()
	for val_idx, val in enumerate(values):
		
		assign_value(val, vector_idx, vector_type, img_cpy)
		if vector_type == 'r':
			col_poss_copy = [v.copy() for v in col_poss]
			revise_cols(col_poss_copy, img_cpy)
			indicies.append((len(col_poss_copy), val_idx))

		else:
			row_poss_copy = [v.copy() for v in row_poss]
			revise_rows(row_poss_copy, img_cpy)
			indicies.append((len(row_poss_copy), val_idx))
		
		#revise(row_poss_copy, col_poss_copy, img_cpy)
		#indicies.append((img_cpy.sum(), val_idx))

	indicies.sort(reverse=True)
	return [idx for val, idx in indicies]

def sort_values2(row_poss, col_poss, values, vector_idx, vector_type, image):
	indicies = []
	for val_idx, val in enumerate(values):
		img_cpy = image.copy()
		assign_value(val, vector_idx, vector_type, img_cpy)
		if vector_type == 'r':
			row_poss_copy = [v.copy() for v in row_poss]
		col_poss_copy = [v.copy() for v in col_poss]
		revise(row_poss_copy, col_poss_copy, img_cpy)
		indicies.append((img_cpy.sum(), val_idx))

	indicies.sort(reverse=True)
	return [idx for val, idx in indicies]


def block_domains(sequence_len, block_lens):
    block_num = len(block_lens)
    q = deque() # queue holdings states. Each state is list of positions of blocks.
    
    positions = []

    pos = 0
    while pos + sum(block_lens) + len(block_lens) - 1 <= sequence_len:
        q.append([pos])
        pos += 1
     
    while not (len(q) == 0):
        state = q.pop()
        state_len = len(state)
        if state_len != block_num:
            
            pos = state[-1] + block_lens[state_len-1] + 1
            while pos + sum(block_lens[state_len:]) + len(block_lens[state_len:]) - 1 <= sequence_len:
                new_state = list(state)
                new_state.append(pos)
                q.append(new_state)
                pos += 1
        else:
            positions.append(state)
    return positions

def binary_descripts(domain, lengths, sequence_len):
	binary_domain = []
	for positions in domain:
		seq = [0] * sequence_len
		for pos, length in zip(positions, lengths):
			for i in range(pos, pos+length):
				seq[i] = 1
		binary_domain.append(seq)
	return binary_domain


def draw_image(image):
    for row in image:
        signs = []
        for i in row:
            signs.append('#' if i == 1 else '.') 
        f_out.write(''.join(signs) + '\n')

def print_image(image):
    for row in image:
        signs = []
        for i in row:
            signs.append('#' if i == 1 else '.') 
        print(''.join(signs) + '\n')
    print('\n*******\n')


f = open("zad_input.txt", 'r')
f_out = open("zad_output.txt", 'w')

row_descriptions = []
col_descriptions = []
for i,l in enumerate(f):
    l = l.split()

    if i == 0:
        row_num = int(l[0])
        col_num = int(l[1])
    elif len(row_descriptions) < row_num:
        row_descriptions.append([int(x) for x in l])
    else:
        col_descriptions.append([int(x) for x in l])


solve(row_descriptions,col_descriptions)
print(total_time)