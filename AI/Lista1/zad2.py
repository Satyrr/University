def insert_spaces(words, no_space_text):
    position_points = [(0,0) for _ in range(len(no_space_text) + 1)] # (points, prev_pos)
    position_queue = [0]
    
    text_len = len(no_space_text)
    checked_positions = {}
    for pos in position_queue:
        for l in range(1,max_word_len):
            if pos + l > text_len: break
            if no_space_text[pos:pos+l] in words:
                if not (pos+l in checked_positions):
               		checked_positions[pos+l] = None
                	position_queue.append(pos+l)
                if position_points[pos+l][0] < position_points[pos][0]+l**2:
                    position_points[pos+l] = (position_points[pos][0]+l**2, pos) 
    idx = position_points[-1][1]
    text = [no_space_text[idx:]]
    while idx != 0:
        text = [no_space_text[position_points[idx][1]:idx]] + text
        idx = position_points[idx][1]
    return ' '.join(text)

f = open("zad2_input.txt", 'r')
f_out = open("zad2_output.txt", 'w')

words = set([ w.rstrip() for w in open('words_for_ai1.txt', 'r')])
max_word_len = max([len(w) for w in words])
pan_tadeusz = [ w.rstrip() for w in f]

pan_tadeusz = [insert_spaces(words, line) for line in pan_tadeusz]
pan_tadeusz = '\n'.join(pan_tadeusz)

f_out.write(pan_tadeusz)


