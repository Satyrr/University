import math

def opt_dist(seq, block_len):
    min_swaps = 99999999999
    for pos in range(len(seq)-block_len+1):
        swaps = sum(seq[:pos]) + sum(seq[pos+block_len:]) + block_len - sum(seq[pos:pos+block_len])
        if(swaps < min_swaps ):
            min_swaps = swaps
    return min_swaps


f = open("zad4_input.txt", 'r')
f_out = open("zad4_output.txt", 'w')

for l in f:
	seq = l.split()[0] 
	seq = [int(c) for c in seq]
	block_len = int(l.split()[1])
	
	f_out.write(str(opt_dist(seq, block_len)) + '\n')