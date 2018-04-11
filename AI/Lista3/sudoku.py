import sys


def V(i,j):
    return 'V%d_%d' % (i,j)
    
def domains(Vs):
    return [ q + ' in 1..9' for q in Vs ]
    
def all_different(Qs):
    return 'all_distinct([' + ', '.join(Qs) + '])'
    
def get_column(j):
    return [V(i,j) for i in range(9)] 
            
def get_raw(i):
    return [V(i,j) for j in range(9)] 

def get_region(i,j):
    region = []
    for col in range(3):
        for row in range(3):
            region.append(V(3*i+row,3*j+col))
    return region


def horizontal():   
    return [ all_different(get_raw(i)) for i in range(9)]

def vertical():
    return [all_different(get_column(j)) for j in range(9)]

def regional():
    regions = []
    for hori_num in range(3):
        for vert_num in range(3):
            regions.append(all_different(get_region(vert_num, hori_num)))
    return regions

def print_constraints(Cs, indent, d):
    position = indent
    f_out.write ((indent - 1) * ' '),
    for c in Cs:
        f_out.write (c + ',')
        position += len(c)
        if position > d:
            position = indent
            f_out.write("\n")
            f_out.write ((indent - 1) * ' '),

      
def sudoku(assigments):
    variables = [ V(i,j) for i in range(9) for j in range(9)]
    
    f_out.write( ':- use_module(library(clpfd)). \n')
    f_out.write('solve([' + ', '.join(variables) + ']) :- \n')
    
    
    cs = domains(variables) + vertical() + horizontal() + regional() #TODO: too weak contraints, add something!
    for i,j,val in assigments:
        cs.append( '%s #= %d' % (V(i,j), val) )
    
    print_constraints(cs, 4, 70),
    f_out.write("\n")
    f_out.write(  '    labeling([ff], [' +  ', '.join(variables) + ']).' )
    f_out.write("\n")
    f_out.write(':- solve(X),open("prolog_result.txt",write, Stream), write(Stream, X), close(Stream), write(X), nl.' )

f = open("zad_input.txt", 'r')
f_out = open("zad_output.txt", 'w')
raw = 0
triples = []
for x in f:
    x = x.strip()
    if len(x) == 9:
        for i in range(9):
            if x[i] != '.':
                triples.append( (raw,i,int(x[i])) ) 
        raw += 1          
sudoku(triples)

"""
89.356.1.
3...1.49.
....2985.
9.7.6432.
.........
.6389.1.4
.3298....
.78.4....
.5.637.48

53..7....
6..195...
.98....6.
8...6...3
4..8.3..1
7...2...6
.6....28.
...419..5
....8..79

3.......1
4..386...
.....1.4.
6.924..3.
..3......
......719
........6
2.7...3..
"""    
