import random
import numpy as np
import math
import time
from collections import deque

ticks = 0
total_time = 0.0

def possible_block_positions(seq_len, block_lens):
    block_num = len(block_lens)
    # queue holdings states. Each state is list of positions of blocks.
    q = deque() 
    pos = 0
    positions = []
    while pos + sum(block_lens) + len(block_lens) - 1 <= seq_len:
        q.append([(pos, block_lens[0])])
        pos += 1
     
    while not (len(q) == 0):
        state = q.pop()
        state_len = len(state)

        if state_len != block_num:
            # new possible position: last block pos + last block len + 1
            pos = state[-1][0] + state[-1][1] + 1
            remaining_pixels = sum(block_lens[state_len:])
            remaining_blocks = len(block_lens[state_len:])
            while pos + remaining_pixels + remaining_blocks - 1 <= seq_len:
                new_state = list(state)
                new_state.append((pos, block_lens[state_len]))
                q.append(new_state)
                pos += 1
        else:
            x = np.zeros(seq_len)
            for s in state:
                x[s[0]:s[0]+s[1]] = 1
            positions.append(x)

    return np.vstack(positions)

def opt_dist(seq, states):
    seq = np.array(seq)
    states = np.copy(states)
    return np.amin(np.abs(states - seq).sum(axis=1))

def random_fill(image):
    for row in image:
        for i in range(len(row)):
            row[i] = random.randint(0,1)
            
def print_to_file(image):
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

# returns list of rows and columns with invalid blocks
def get_invalid_rows_cols(col_opt_dists, row_opt_dists):
    invalid = []
    for row_num in range(len(row_opt_dists)):
        if row_opt_dists[row_num] > 0:
            invalid.append((row_num, -1))
            
    for col_num in range(len(col_opt_dists)):
        if col_opt_dists[col_num] > 0:
            invalid.append((-1, col_num))    
            
    return invalid

# fixes pixels which are '#' in all possible blocks
def fix_positions(descripts, rows, cols, fix_rows):
    solid = np.ones((rows, cols))
    for idx in range(len(descripts)):
        mat = np.any(descripts[idx] == 0, axis=0)
        if fix_rows:
            solid[idx, mat] = 0
        else:
            solid[mat, idx] = 0
    return solid

def WalkSat(descriptions, draw=False):
    rows, cols = len(descriptions[0]), len(descriptions[1])

    image = np.random.random_integers(0, 1 , size=(rows,cols))

    rows_block_positions = [possible_block_positions(cols, r) 
        for r in descriptions[0]]
    cols_block_positions = [possible_block_positions(rows, c) 
        for c in descriptions[1]]

    fixed_by_row = fix_positions(rows_block_positions, rows, cols, True)
    fixed_by_col = fix_positions(cols_block_positions, rows, cols, False)
    fixed = fixed_by_col + fixed_by_row
    image[fixed > 0] = 1
    
    col_opt_dists = [opt_dist(image[:, i], cols_block_positions[i]) 
        for i in range(cols)]
    row_opt_dists = [opt_dist(image[i, :], rows_block_positions[i]) 
        for i in range(rows)]

    iterations = 0
    while True:
        invalid_rc = get_invalid_rows_cols(col_opt_dists, row_opt_dists)

        if len(invalid_rc) == 0:
            print('found in: ', iterations)
            break

        rnd = random.choice(invalid_rc)
        min_dist, min_dist_pos = 999999, (-1,-1)
        num = 0

        # go through all pixels in row/column
        while num < (rows if rnd[0] == -1 else cols): 
            row_num = rnd[0] if rnd[1] == -1 else num
            col_num = rnd[1] if rnd[0] == -1 else num

            if fixed[row_num, col_num] > 0: 
                num += 1
                continue

            row = image[row_num,:].tolist()
            col = image[:, col_num].tolist()
            old_row_dist = row_opt_dists[row_num]
            old_col_dist = col_opt_dists[col_num]
            old_dist = old_row_dist + old_col_dist

            col[row_num] = 1 - col[row_num] 
            row[col_num] = 1 - row[col_num]

            new_row_dist = opt_dist(row, rows_block_positions[row_num])
            new_col_dist = opt_dist(col, cols_block_positions[col_num])
            dist = new_row_dist + new_col_dist

            if dist - old_dist == min_dist and old_row_dist > new_row_dist \
                and old_col_dist > new_col_dist and random.random() > 0.5:
                min_dist_pos = (row_num, col_num)
            if dist - old_dist < min_dist and old_row_dist >= new_row_dist \
                and old_col_dist >= new_col_dist:
                min_dist_pos = (row_num, col_num)
                min_dist = dist - old_dist
            elif dist - old_dist < min_dist and np.random.rand() < 0.20: # 0.09
                min_dist_pos = (row_num, col_num)
                min_dist = dist - old_dist
            
            num += 1

        if min_dist_pos == (-1,-1): continue

        # negate pixel
        image[min_dist_pos] = 1 - image[min_dist_pos]

        # recompute opt dists
        idx = min_dist_pos[0]
        row_opt_dists[idx] = opt_dist(image[idx, :], rows_block_positions[idx])
        idx = min_dist_pos[1]
        col_opt_dists[idx] = opt_dist(image[:, idx], cols_block_positions[idx])
        
        iterations += 1
        if iterations > rows*cols*200:
            print_image(image)
            image = np.random.random_integers(0, 1 , size=(rows,cols))
            image[fixed > 0] = 1
            iterations = 0
            
    print_to_file(image)


f = open("zad_input.txt", 'r')
f_out = open("zad_output.txt", 'w')

row = []
col = []
for i,l in enumerate(f):
    l = l.split()

    if i == 0:
        row_num = int(l[0])
        col_num = int(l[1])
    elif len(row) < row_num:
        row.append([int(x) for x in l])
    else:
        col.append([int(x) for x in l])


WalkSat((row,col))