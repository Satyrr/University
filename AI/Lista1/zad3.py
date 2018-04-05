import itertools as it
import random, time

# warianty: 0 - wysoka karta, 1 - para, 2 - 2 pary, 3 - trojka, 4 - strit, 5 - kolor, 6 - full, 7 - kareta, 8 - poker
def variant(figures, colors = None):
    if colors:
        colors_count = [0] * 4
        for col in colors:
            colors_count[col] += 1
        max_c = max(colors_count)
    
    fig_count = [0]*10
    for k in figures:
        fig_count[k] += 1
    fig_count.sort()
    
    s_figures = sorted(figures)
    
    #poker
    if [f-min(figures) for f in s_figures] == [0,1,2,3,4] and max_c == 5: return 8
    #kareta
    if fig_count[-1] == 4: return 7
    if fig_count[-1] == 3 and fig_count[-2] == 2: return 6
    if colors and max_c == 5: return 5
    if [f-min(figures) for f in s_figures] == [0,1,2,3,4]: return 4
    if fig_count[-1] == 3: return 3
    if fig_count[-1] == 2 and fig_count[-2] == 2: return 2
    if fig_count[-1] == 2: return 1
    return 0

def test(talia_f, talia_b, all_combinations=False):
    won, lost = 0, 0
    
    tf_combinations = list(it.combinations(talia_f, 5))
    tb_combinations = list(it.combinations(talia_b, 5))
    tf_samples = len(tf_combinations) if all_combinations else 1000
    tb_samples = len(tb_combinations) if all_combinations else 1000
    tf_combinations = random.sample(tf_combinations, tf_samples)
    tb_combinations = random.sample(tb_combinations, tb_samples)

    variants_tb = []
    for tb in tb_combinations:
        colors_tb = [col for (fig, col) in tb]
        tb_f = [fig for (fig, col) in tb]
        variants_tb.append(variant(tb_f, colors_tb))
    
    for tf in tf_combinations:
        variant_tf = variant(tf)
        for variant_tb in variants_tb:
            if variant_tb > variant_tf:
                won += 1
            else:
                lost += 1
    print(float(won)/(won+lost))

talia_figurant = (4*[0]) + (4*[1]) + (4*[2]) + (4*[3])
talia_blotkarz = []
for color in range(4):
    for fig in range(9):
        talia_blotkarz.append((fig,color))
print("Talia podstawowa")
#t0 = time.time()
test(talia_figurant, talia_blotkarz)
#print(time.time() - t0)

talia_figurant = (4*[0]) + (4*[1]) + (4*[2]) + (4*[3])
talia_blotkarz = []
for color in range(3):
    for fig in range(9):
        talia_blotkarz.append((fig,color))
print("Talia 3 kolory")
test(talia_figurant, talia_blotkarz)

talia_figurant = (4*[0]) + (4*[1]) + (4*[2]) + (4*[3])
talia_blotkarz = []
for color in range(1):
    for fig in range(5):
        talia_blotkarz.append((fig,color))
print("Talia 1 kolor, 5 figur")
test(talia_figurant, talia_blotkarz, True)

talia_figurant = (4*[0]) + (4*[1]) + (4*[2]) + (4*[3])
talia_blotkarz = []
for color in range(4):
    for fig in range(3):
        talia_blotkarz.append((fig,color))
print("Talia 4 kolory, 3 figury")
test(talia_figurant, talia_blotkarz, True)