import random
import sys
#import numpy as np
from collections import defaultdict as dd
import torch
from torch import nn

M = 8


def initial_board():
    B = [ [-1] * M for i in range(M)]
    B[3][3] = 1
    B[4][4] = 1
    B[3][4] = 0
    B[4][3] = 0
    return B

moves_dict = {}
    
class Board:
    dirs  = [ (0,1), (1,0), (-1,0), (0,-1), (1,1), (-1,-1), (1,-1), (-1,1) ]
    
    
    def __init__(self, starting_board=None): 
        if starting_board == None:
            self.board = initial_board()
            self.fields = set()
            self.move_list = []
            self.dics_num = 4
            self.history = []
            self.new_fields(3,3)
            self.new_fields(4,4)
            self.new_fields(3,4)
            self.new_fields(4,3)
        else:
            self.board = [list(row) for row in starting_board.board]
            self.dics_num = starting_board.dics_num
            self.fields = set(starting_board.fields)
            self.move_list = list(starting_board.move_list)
            self.history = []
        
    
    def new_fields(self, x,y):
        for d in self.dirs:
            x0, y0 = d
            neighbour = self.get(x+x0,y+y0)
            if neighbour != None and neighbour < 0:
                self.fields.add( (x+x0,y+y0) )

    def draw(self):
        for i in range(M):
            res = []
            for j in range(M):
                b = self.board[i][j]
                if b == -1:
                    res.append('.')
                elif b == 1:
                    res.append('#')
                else:
                    res.append('o')
            print ('').join(res) 
        print            
        
    
    def show(self):
        for i in range(M):
            for j in range(M):
                kwadrat(j, i, 'green')
                
        for i in range(M):
            for j in range(M):                
                if self.board[i][j] == 1:
                    kolko(j, i, 'black')
                if self.board[i][j] == 0:
                    kolko(j, i, 'white')
                                   
    def moves(self, player):
        res = []

        for (x,y) in self.fields:
            if any( self.can_beat(x,y, direction, player) 
                for direction in Board.dirs):
                res.append( (x,y) )
        
        if not res:
            return [None]

        return res               
    
    def can_beat(self, x,y, d, player):
        dx,dy = d
        x += dx
        y += dy
        cnt = 0
        while self.get(x,y) == 1-player:
            x += dx
            y += dy
            cnt += 1
        return cnt > 0 and self.get(x,y) == player
    
    def get(self, x,y):
        if 0 <= x < M and 0 <=y < M:
            return self.board[x][y]
        return None
                        
    def do_move(self, move, player):
        #self.history.append([x[:] for x in self.board])
        self.move_list.append(move)

        if move == None:
            return
        x,y = move
        x0,y0 = move
        self.board[x][y] = player
        self.new_fields(x,y)
        self.fields -= set([move])
        self.dics_num += 1
        for dx,dy in self.dirs:
            x,y = x0,y0
            to_beat = []
            x += dx
            y += dy
            while self.get(x,y) == 1-player:
              to_beat.append( (x,y) )
              x += dx
              y += dy
            if self.get(x,y) == player:              
                for (nx,ny) in to_beat:
                    self.board[nx][ny] = player
                                                     
    def result(self):
        res = 0
        for y in range(M):
            for x in range(M):
                b = self.board[x][y]                
                if b == 0:
                    res -= 1
                elif b == 1:
                    res += 1
        return res
                
    def terminal(self):
        if not self.fields:
            return True
        if len(self.move_list) < 2:
            return False
        return self.move_list[-1] == self.move_list[-2] == None 

    def experiment(self, move_selector_max, move_selector_min):
        player = 1
        global total_random
        global total_move
        while True:

            if player == 0:
                m = move_selector_min(self, player)
            else:
                m = move_selector_max(self, player)

            self.do_move(m, player)

            player = 1-player
            if self.terminal():
                break

        return self.result()

def random_move(board, player):
    ms = board.moves(player)
    
    if ms:
        return random.choice(ms)
    return None

def alfabeta_move(board, player):
    ms = board.moves(player)

    if ms[0] == None:
        return None

    minmax_instance = AlfaBeta()
    if len(board.move_list) < 40:
        return minmax_instance.decision_max(board, player, 0)
    elif len(board.move_list) < 55:
        return minmax_instance.decision_max(board, player, 0)
    else:
        end_game = True
        return minmax_instance.decision_max(board, player, 0)

end_game = False
class AlfaBeta:

    def decision_max(self, starting_B, player, max_depth=2):
        self.max_depth = max_depth
        moves = starting_B.moves(player)
        if moves[0] == None:
            return moves

        states = []
        for m in moves:
            B = Board(starting_B)
            B.do_move(m, player)
            states.append(B)

        alfa = -9999999
        beta = 9999999
        for m, s in zip(moves, states):
            value = self.min_value(s, 1-player, 0, alfa, beta)
            if value >= alfa:
                best_move = m
            alfa = max(alfa, value)

        return best_move


    def min_value(self, B, player, depth, alpha, beta):

        if depth==self.max_depth or B.terminal():
            return heuristic_value(B, player)
        moves = B.moves(player)
        if moves[0] == None:
            return heuristic_value(B, player)


        value = 999999
        for m in moves:
            B_result = Board(B)
            B_result.do_move(m, player)
            
            value = min(value,
                self.max_value(B_result, 1-player, depth+1, alpha, beta))
            if value <= alpha:
                return value
            beta = min(beta, value)

        return value
        

    def max_value(self, B, player, depth, alpha, beta):

        if depth==self.max_depth or B.terminal():
            return heuristic_value(B, player)

        moves = B.moves(player)
        if moves[0] == None:
            return heuristic_value(B, player)
        
        value = -999999
        for m in moves:
            B_result = Board(B)
            B_result.do_move(m, player)
            
            value = max(value,
                self.min_value(B_result, 1-player, depth+1, alpha, beta))
            if value >= beta:
                return value
            alpha = max(alpha, value)

        return value

###########################################################
# Heuristics

def front_tiles_heuristic(B):
    res = 0
    checked = set()
    min_front_tiles = 0
    max_front_tiles = 0
    for x, y in B.fields:
        for d in B.dirs:
            dx, dy = d
            if B.get(x+dx, y+dy) == 1 and not (x+dx, y+dy) in checked:
                max_front_tiles += 1
                checked.add((x+dx, y+dy))
                break
            if B.get(x+dx, y+dy) == 0 and not (x+dx, y+dy) in checked:
                min_front_tiles += 1
                checked.add((x+dx, y+dy))
                break
    if max_front_tiles + min_front_tiles == 0: return 0

    return 100.0*float(min_front_tiles - max_front_tiles)/ \
        (max_front_tiles + min_front_tiles )

def coin_parity_heuristic(B):
    res = 0
    total = 0
    for y in range(M):
        for x in range(M):
            b = B.board[x][y]                
            if b == 0:
                res -= 1
                total += 1
            elif b == 1:
                res += 1
                total += 1

    return 100.0*float(res)/total

def corners_heuristic(B):
    res = 0
    total = 0
    for cor_x, cor_y in [(0,0), (0,7), (7,0), (7,7)]:
        if B.board[cor_x][cor_y] == 0:
            res -= 1
        elif B.board[cor_x][cor_y] == 1:
            res += 1

    return 25*res

def corners_closneness_heuristic(B):
    corners = [(0,0), (0,7), (7,0), (7,7)]
    nears = [[(0,1), (1,0), (1,1)],
        [(1,7), (0,6), (1,6)],
        [(7,1), (6,0), (6,1)],
        [(7,6), (6,6), (6,7)]]
    res = 0
    for (cor_x, cor_y), near in zip(corners, nears):
        if B.board[cor_x][cor_y] != -1: continue
        for near_x, near_y in near:
            if B.board[near_x][near_y] == 0:
                res += 1
            elif B.board[near_x][near_y] == 1:
                res -= 1

    return 8*res


def mobility_heuristic(B):
    max_moves = number_of_moves(B, 1)
    min_moves = number_of_moves(B, 0)
    if max_moves + min_moves == 0:
        mobility = 0
    else:
        mobility = 100.0*float((max_moves - min_moves))/(max_moves + min_moves)
    return mobility

def number_of_moves(B, player):
    moves = B.moves(player)
    if moves[0] == None:
        return 0 
    return len(moves)


def heuristic_value(B, player):
    ## mobility
    mobility = mobility_heuristic(B)
    ## corners
    cors = corners_heuristic(B)
    ## corner closeness
    closeness = corners_closneness_heuristic(B)
    ## stability
    #stability = front_tiles_heuristic(B)

    result = 0
    if end_game:
        ## coin parity
        coin_p = coin_parity_heuristic(B)
        result += 20*coin_p
    #return 78.922*mobility + 10*coin_p + 801.724*cors + \
    # 74.396*stability + 10*res + l
    return result + 200*cors  + 150 * closeness + 20 * mobility

#####################################
# Neural Network
class Reversi_net(nn.Module):
    def __init__(self):
        super(Reversi_net, self).__init__()
        self.layer1 = nn.Sequential(
            nn.Linear(128, 1200),
            nn.Dropout(0.5),
            nn.ReLU()
            )
        self.layer2 = nn.Sequential(
            nn.Linear(1200, 200),
            nn.Dropout(0.5),
            nn.ReLU()
            )
        self.fc2 = nn.Linear(200, 1)
        
    def forward(self, x):
        out = x.view(x.size(0), -1)
        out = self.layer1(out)
        out = self.layer2(out)
        out = self.fc2(out)
        return out
    
class Reversi_simple_heuristic_net(nn.Module):
    def __init__(self):
        super(Reversi_simple_heuristic_net, self).__init__()
        self.layer1 = nn.Sequential(
            nn.Linear(3, 700),
            nn.Dropout(0.5),
            nn.ReLU()
            )
        self.layer2 = nn.Sequential(
            nn.Linear(700, 400),
            nn.Dropout(0.5),
            nn.ReLU()
            )
        self.fc2 = nn.Linear(400, 1)
        
    def forward(self, x):
        out = x.view(x.size(0), -1)
        out = self.layer1(out)
        out = self.layer2(out)
        out = self.fc2(out)
        return out
    
class Reversi_heuristic_net(nn.Module):
    def __init__(self):
        super(Reversi_heuristic_net, self).__init__()
        self.layer1 = nn.Sequential(
            nn.Linear(5, 700),
            nn.Dropout(0.5),
            nn.ReLU()
            )
        self.layer2 = nn.Sequential(
            nn.Linear(700, 400),
            nn.Dropout(0.5),
            nn.ReLU()
            )
        self.fc2 = nn.Linear(400, 1)
        
    def forward(self, x):
        out = x.view(x.size(0), -1)
        out = self.layer1(out)
        out = self.layer2(out)
        out = self.fc2(out)
        return out

def best_nn_move(board, player, value_function):
    moves = board.moves(player)
    if moves[0] == None:
        return None

    states = []
    for m in moves:
        B = Board(board)
        B.do_move(m, player)
        states.append(B)

    values = [value_function(s) for s in states]

    if player == 0:
        _, best_move =  min([v,m] for (v,m) in zip(values, moves))
    else:
        _, best_move =  max([v,m] for (v,m) in zip(values, moves))

    return best_move

def nn_board_value(board):
    coded_board = torch.zeros((2,8,8))
    for r in range(8):
        for c in range(8):
            if board.board[r][c] == 0:
                coded_board[0,r,c] = 1
            elif board.board[r][c] == 1:
                coded_board[1,r,c] = 1

    coded_board = coded_board.view(1,-1)

    val = the_model_1.forward(coded_board)
    return val.item()

def nn_simple_heuristic_value(board):
    heurs = torch.tensor([coin_parity_heuristic(board), 
        corners_heuristic(board),
        corners_closneness_heuristic(board)]).view(1,-1)

    return the_model_2.forward(heurs).item()

def nn_heuristic_value(board):
    heurs = torch.tensor([coin_parity_heuristic(board), 
        corners_heuristic(board),
        corners_closneness_heuristic(board),
        mobility_heuristic(board),
        front_tiles_heuristic(board)]).view(1,-1)

    return the_model_3.forward(heurs).item()


#949 wins/1000 games
the_model_1 = Reversi_net()
the_model_1.load_state_dict(torch.load("simple_features_cpu"))
the_model_1.eval()

#961 wins/1000 games
#parity, cors, cors_n
the_model_2 = Reversi_simple_heuristic_net()
the_model_2.load_state_dict(torch.load("simple_heuristic_features_cpu"))
the_model_2.eval()

#parity, cors, cors_n, mobil, fronts
the_model_3 = Reversi_heuristic_net()
the_model_3.load_state_dict(torch.load("heuristic_features_cpu"))
the_model_3.eval()

#####################################
import time 
wins = 0
lost = 0
for iteration in range(100):
    B = Board()
    res = B.experiment(random_move, 
        lambda b, p: best_nn_move(b, p, nn_heuristic_value))
    #res = B.experiment(lambda b, p: best_nn_move(b, p, nn_simple_heuristic_value), 
        #lambda b, p: best_nn_move(b, p, nn_heuristic_value))
    #B.draw()
    if res <= 0:
        wins += 1
    else:
        lost += 1
    if iteration % 40 == 0: print(iteration)

print('NN Losts: %d' % lost)
print('NN Wins: %d' % wins)
print('Win rate : %d' % (100.0*wins/(wins+lost)))
