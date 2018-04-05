from collections import deque

#eg. 'h4' -> (3,7)
def decode_indicies(pos):
    return (int(pos[1])-1, ord(pos[0])-97)

#eg. (3,7) -> 'h4' 
def encode_indicies(pos):
    return chr(pos[1]+97) + chr(pos[0]+49) 

def king_possible_moves(position, state):
    positions = []
    for x in range(-1, 2):
        for y in range(-1, 2):
            new_pos = (position[0] + x, position[1] + y)
            if(min(new_pos) >= 0 and max(new_pos) < 8):
                positions.append(new_pos)
                
    return positions
    
def rook_possible_moves(position, state):
    positions = []
    for i in range(1, 8):
        new_pos_r = (position[0] + i, position[1])
        new_pos_l = (position[0] - i, position[1])
        new_pos_t = (position[0], position[1] + i)
        new_pos_b = (position[0], position[1] - i)
        
        if not (max(new_pos_r) > 7 or new_pos_r == state[0]):
            positions.append(new_pos_r) 
        if not (min(new_pos_l) < 0 or new_pos_l == state[0]):
            positions.append(new_pos_l) 
        if not (max(new_pos_t) > 7 or new_pos_t == state[0]):
            positions.append(new_pos_t) 
        if not (min(new_pos_b) < 0 or new_pos_b == state[0]):
            positions.append(new_pos_b) 
        
    return positions       
    
def is_checked(state, player, w_rook_positions):
    kings_distance = (state[0][0]-state[2][0], state[0][1]-state[2][1])
    kings_distance = (kings_distance[0]**2, kings_distance[1]**2)
    if player == 'w':
        return sum(kings_distance) <= 2
    else:
        return sum(kings_distance) <= 2 or \
            state[2] in w_rook_positions
    
def get_possible_states(state, player):
    """Zwraca stany po wykonaniu mozliwych ruchow aktualnego gracza"""
    b_king_positions = king_possible_moves(state[2], state)
    w_king_positions = king_possible_moves(state[0], state)
    w_rook_positions = rook_possible_moves(state[1], state)

    if player == 'w':
        states = [ [k, state[1], state[2]] for k in w_king_positions ] \
            + [ [state[0], r, state[2]] for r in w_rook_positions ]
    else:
        states = [ [state[0], state[1], k] for k in b_king_positions ]
        
    states = [st for st in states if not is_checked(st, player, w_rook_positions)]
    return states

#0 - white king, 1 - white rook, 2 - black king


def moves_to_checkmate(pieces, player):
    queue = deque([(pieces, player)]) #[(pieces, player)]

    checked_states = {str((pieces, player)):None}
    while True:
        current_state, player = queue.pop()
        possible_states = get_possible_states(current_state, player)

        #checkmate
        if(len(possible_states) == 0 and \
           is_checked(current_state, 'b', rook_possible_moves(current_state[1], current_state))): 
            break 
    
        second_player = 'b' if player == 'w' else 'w'
        for state in possible_states:
            if(str((state, second_player)) not in checked_states and state[1] != state[2]):
                checked_states[str((state, second_player))] = (current_state, player)
                queue.appendleft((state,second_player))
        
        player = second_player

    moves = []
    while checked_states[str((current_state, player))] is not None:
        board = [encode_indicies(pos) for pos in current_state]
        moves.append(board)
        current_state,player = checked_states[str((current_state, player))]
    
    return moves
    

draw_moves = True

f = open("zad1_input.txt", 'r')
f_out = open("zad1_output.txt", 'w')
for line in f:

    settings = line.split()
    player = settings[0][0]
    pieces = [decode_indicies(s) for s in settings[1:]]
    moves = moves_to_checkmate(pieces, player)
    
    if draw_moves:
        for move in reversed(moves):
            print(move)
            #print_board(move[0], move[1], move[2]) 
        print(str(len(moves)))
        f_out.write(str(len(moves)))
