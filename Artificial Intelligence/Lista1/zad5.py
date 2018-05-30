import random
import numpy as np

import math
from collections import deque

def possible_block_positions(seq_len, block_lens):
    block_num = len(block_lens)
    q = deque() # queue holdings states. Each state is list of positions of blocks.
    pos = 0
    positions = []
    while pos + sum(block_lens) + len(block_lens) - 1 <= seq_len:
        q.append([pos])
        pos += 1
     
    while not (len(q) == 0):
        state = q.pop()
        state_len = len(state)
        if state_len != block_num:
            
            pos = state[-1] + block_lens[state_len-1] + 1
            while pos + sum(block_lens[state_len:]) + len(block_lens[state_len:]) - 1 <= seq_len:
                new_state = list(state)
                new_state.append(pos)
                q.append(new_state)
                pos += 1
        else:
            positions.append(state)
    return positions

def opt_dist(seq, states, block_lens):
    min_swaps = 999999
    seq_len = len(seq)
    seq_sum = sum(seq)
    block_num = len(block_lens)
    
    for state in states:
        swaps = 0
        for pos, bl in zip(state, block_lens):
            swaps += bl - sum(seq[pos:pos+bl])
        swaps += seq_sum + swaps - sum(block_lens)
        if(swaps < min_swaps ):
            min_swaps = swaps
        if min_swaps == 0:
            return 0
    return min_swaps

def random_fill(image):
    for row in image:
        for i in range(len(row)):
            row[i] = random.randint(0,1)
            
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

    
def fix_positions(block_positions, descripts, rows, cols, fix_rows):
    solid = np.ones((rows, cols))
    for idx in range(len(block_positions)):
        for positions_list in block_positions[idx]:
            if fix_rows:
                mat = np.zeros(cols)
            else :
                mat = np.zeros(rows)
            for i, pos in enumerate(positions_list):
                mat[pos:pos+descripts[idx][i]] = 1
            if fix_rows:
                solid[idx, mat==0] = 0
            else:
                solid[mat==0, idx] = 0
    return solid

def WalkSat(descriptions, random_init=True, draw=False):
    for img_num in range(len(descriptions)):
        rows, cols = len(descriptions[img_num][0]), len(descriptions[img_num][1])
        
        rows_block_positions = [possible_block_positions(cols, r) for r in descriptions[img_num][0]]
        cols_block_positions = [possible_block_positions(rows, c) for c in descriptions[img_num][1]]

        fixed_by_row = fix_positions(rows_block_positions, descriptions[img_num][0], rows, cols, True)
        fixed_by_col = fix_positions(cols_block_positions, descriptions[img_num][1], rows, cols, False)
        fixed = fixed_by_col + fixed_by_row
        if random_init:
            image = np.random.random_integers(0, 1 , size=(rows,cols))
        else:
            image = np.zeros((rows, cols))
        image[fixed > 0] = 1
        
        col_opt_dists = [opt_dist(image[:, i], cols_block_positions[i], descriptions[img_num][1][i]) for i in range(cols)]
        row_opt_dists = [opt_dist(image[i, :], rows_block_positions[i], descriptions[img_num][0][i]) for i in range(rows)]
        iterations = 0
        while True:
            invalid_rc = get_invalid_rows_cols(col_opt_dists, row_opt_dists)
            if len(invalid_rc) == 0:
                break

            rnd = random.choice(invalid_rc)

            min_dist, min_dist_pos = 999999, (-1,-1)
            num = 0

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
                orig_dist =  + old_row_dist + old_col_dist

                col[row_num] = (col[row_num] + 1) % 2
                row[col_num] = (row[col_num] + 1) % 2
                new_row_dist = opt_dist(row, rows_block_positions[row_num], descriptions[img_num][0][row_num])
                new_col_dist = opt_dist(col, cols_block_positions[col_num], descriptions[img_num][1][col_num])
                dist = new_row_dist + new_col_dist

                if dist - orig_dist == min_dist and old_row_dist > new_row_dist and old_col_dist > new_col_dist and random.random() > 0.5:
                        min_dist_pos = (row_num, col_num)
                if dist - orig_dist < min_dist and old_row_dist > new_row_dist and old_col_dist > new_col_dist:
                    min_dist_pos = (row_num, col_num)
                    min_dist = dist - orig_dist
                if dist - orig_dist < min_dist and np.random.rand() < 0.15: # 0.09
                        min_dist_pos = (row_num, col_num)
                        min_dist = dist - orig_dist
                num += 1

            if min_dist_pos == (-1,-1): continue

            image[min_dist_pos] = (image[min_dist_pos] + 1) % 2

            idx = min_dist_pos[0]
            row_opt_dists[idx] = opt_dist(image[idx, :], rows_block_positions[idx], descriptions[img_num][0][idx])
            idx = min_dist_pos[1]
            col_opt_dists[idx] = opt_dist(image[:, idx], cols_block_positions[idx], descriptions[img_num][1][idx])
            
            iterations += 1

            if iterations > rows*cols*40:
                image = np.random.random_integers(0, 1 , size=(rows,cols))
                image[fixed > 0] = 1
                iterations = 0
        draw_image(image)


f = open("zad5_input.txt", 'r')
f_out = open("zad5_output.txt", 'w')

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


WalkSat([(row,col)])