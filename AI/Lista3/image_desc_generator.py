def image_case(rows_desc, cols_desc, image):
    description = "- inp: |\n%d %d\n" % (len(rows_desc), len(cols_desc))

    for row in rows_desc:
        for num in row:
            description += "%d " % num
        description += '\n'
    
    for col in cols_desc:
        for num in col:
            description += "%d " % num
        description += '\n'

    description += "out: |\n"

    description += image
    return description

def image_desc(image):
    image = image.strip().split('\n')
    image = [list(row.strip()) for row in image]
    
    rows_desc = []
    cols_desc = []

    for r in range(len(image)):
        cur_block = 0
        rows_desc.append([])
        for c in range(len(image[0])):
            if image[r][c] == '.' and cur_block > 0:
                rows_desc[r].append(cur_block)
                cur_block = 0
            if image[r][c] == '#':
                cur_block += 1
        
        if cur_block != 0:
            rows_desc[r].append(cur_block)
            cur_block = 0
        if len(rows_desc[r]) == 0:
            rows_desc[r].append(0)
        
    for c in range(len(image[0])):
        cur_block = 0
        cols_desc.append([])
        for r in range(len(image)):
            if image[r][c] == '.' and cur_block > 0:
                cols_desc[c].append(cur_block)
                cur_block = 0
            if image[r][c] == '#':
                cur_block += 1
        
        if cur_block != 0:
            cols_desc[c].append(cur_block)
            cur_block = 0
            
        if len(cols_desc[c]) == 0:
            cols_desc[c].append(0)

    return rows_desc, cols_desc

img = """........................................
...........................#########....
........................#############...
.................##############.....##..
...............##############........#..
.............##################.......#.
...........######################.....#.
..........########################....#.
.........##########################...#.
........############################....
.......####.##########################..
.......##..#########....##############..
......##..########........############..
.....##..#######...........###########..
.....#..########............###########.
.......########..............##########.
.......########..............##########.
......########...............###########
.....###################################
.....###################################
....####################################
....####################################
...#####################################
...###########..........................
..############..........................
..#############.........................
.##############.........................
.###############.............##########.
.###.###########............##########..
####..############.........###########..
###....############......############...
###....##############################...
###.....############################....
###......##########################.....
###.......########################......
###........######################.......
.##.........###################.........
.####.....###.###############...........
..########.......##########.............
........................................
"""
rows_d, cols_d = image_desc(img)

print(image_case(rows_d, cols_d, img))