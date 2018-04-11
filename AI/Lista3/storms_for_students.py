
def B(i,j):
    return 'B_%d_%d' % (i,j)

def squares():
    cons = []
    for x in range(len(raws)-1):
        for y in range(len(cols)-1):
            cons.append( '(' + '+'.join([B(x,y), B(x,y+1), B(x+1,y), B(x+1,y+1)]) + ') #\= 3')    
    return cons

def squares3():
    cons = []
    for x in range(len(raws)-1):
        for y in range(len(cols)-1):
            cons.append( '(' + '+'.join([B(x,y), B(x+1,y+1)]) + ') #= 2 #<==> (' + '+'.join([B(x,y+1), B(x+1,y)]) + ') #= 2') 
    return cons

def squares2():
    cons = []
    for x in range(len(raws)-1):
        for y in range(len(cols)-1):
            cons.append( '(' + '+'.join([B(x,y), B(x+1,y+1)]) + ') #\= 2')
            cons.append( '(' + '+'.join([B(x,y+1), B(x+1,y)]) + ') #\= 2')    
    return cons

def triples_cs():
    cons = []
    for x in range(len(raws)):
        for y in range(len(cols)-2):
            cons.append(  B(x,y+1) + ' #= 1 #==> (' + '+'.join([B(x,y), B(x,y+2)]) + ') #> 0')

    for x in range(len(raws)-2):
        for y in range(len(cols)):
            cons.append(  B(x+1,y) + ' #= 1 #==> (' + '+'.join([B(x,y), B(x+2,y)]) + ') #> 0')
    return cons

def domains(Vs):
    return [ q + ' in 0..1' for q in Vs ]

def get_column(j):
    return [B(i,j) for i in range(len(raws))] 
            
def get_raw(i):
    return [B(i,j) for j in range(len(cols))] 

def horizontal():
    return ['+'.join(get_raw(i)) + (' #= %d' % raws[i]) for i in range(len(raws))]

def vertical():
    return ['+'.join(get_column(i))+ (' #= %d' % cols[i] ) for i in range(len(cols))]

def print_constraints(Cs, indent, d):
    position = indent
    write ((indent - 1) * ' '),
    for c in Cs:
        write (c + ', ')
        position += len(c)
        if position > d:
            position = indent
            write("\n")
            write ((indent - 1) * ' '),

def storms(raws, cols, triples):
    writeln(':- use_module(library(clpfd)).')
    
    R = len(raws)
    C = len(cols)
    
    bs = [ B(i,j) for i in range(R) for j in range(C)]
    
    writeln('solve([' + ', '.join(bs) + ']) :- \n')

    #TODO: add some constraints
    cs = domains(bs) + horizontal() + vertical() + squares3() + triples_cs()
    for x, y, val in triples:
        cs.append(B(x,y) + ' #= ' + str(val))
    #writeln('    [%s] = [1,1,0,1,1,0,1,1,0,1,1,0,0,0,0,0,0,0,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,0],' % (', '.join(bs),)) #only for test 1
    print_constraints(cs, 4, 70),

    writeln('')
    writeln('    labeling([ff], [' +  ', '.join(bs) + ']).' )
    writeln('')
    writeln(":- tell('prolog_result.txt'), solve(X), write(X), nl, told.")

def writeln(s):
    output.write(s + '\n')
def write(s):
    output.write(s)


txt = open('zad_input.txt').readlines()
output = open('zad_output.txt', 'w')

raws = list(map(int, txt[0].split()))
cols = list(map(int, txt[1].split()))
triples = []

for i in range(2, len(txt)):
    if txt[i].strip():
        triples.append(map(int, txt[i].split()))

storms(raws, cols, triples)            
        

