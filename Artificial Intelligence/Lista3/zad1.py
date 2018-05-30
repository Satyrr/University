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

def solve(row_descriptions_list, col_descriptions_list):

	row_possible_positions = [np.array(binary_descripts(block_domains(col_num, row_description), row_description, col_num)) \
		for row_description in row_descriptions_list]

	col_possible_positions = [np.array(binary_descripts(block_domains(row_num, col_description), col_description, row_num)) \
		for col_description in col_descriptions_list]

	image = np.ones((row_num, col_num)) * (-1)
	revised = True
	while revised:
		revised = False
		image_sum = image.sum()
		revised |= revise_rows(row_possible_positions, image)
		fix_by_rows(row_possible_positions, image)
		revised |= revise_cols(col_possible_positions, image)
		fix_by_cols(col_possible_positions, image)
		revised |= image_sum < image.sum()

	print_image(image)
	draw_image(image)
		


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

"""
def AC_3(row_possible_positions, col_possible_positions):
	
	queue = deque()
	queue_dict = {}

	for r in range(row_num):
		for c in range(col_num):

			queue.append(('r', r, c))
			queue_dict[('r', r, c)] = True

			queue.append(('c', c, r))
			queue_dict[('c', c, r)] = True

	while queue:
		revise_type, revise_idx, with_idx = q.pop()
		queue_dict[(revise_type, revise_idx, with_idx)] = False

		if revise_type == 'r':
			X = row_possible_positions[revise_idx]
			Y = col_possible_positions[with_idx]
		else:
			X = col_possible_positions[with_idx]
			Y = row_possible_positions[revise_idx]

		if revise(X, revise_idx, Y, with_idx):
			if len(X) == 0 : return False

			opposite_revise_type = 'c' if revise_type == 'r' else 'r'
			for idx in range(col_num if revise_type == 'r' else row_num):
				if idx != with_idx:
					queue.append((opposite_revise_type, idx, revise_idx))
					queue_dict[(opposite_revise_type, idx, revise_idx)] = True

		return True


def revise(blocks1, idx1, blocks2, idx2):
	if all([b[idx1] == 0 for b in block2]):
"""