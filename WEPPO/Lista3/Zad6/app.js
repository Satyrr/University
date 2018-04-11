function createGenerator() {
    var _state = 0;
    var param = this.zmienna;
    return {
        //param : this.zmienna,
        //_state : 0,
        // czy roznica w takim podejsciu to jedynie koniecznosc uzycia this.param, this._state ?
        next: function () {
            return {
                value: _state,
                done: _state++ >= param
            }
        }
    }
}

var foo = {
    [Symbol.iterator]: createGenerator,
    zmienna: 5
};

for (var f of foo)
    console.log(f);
for (var f of foo)
    console.log(f);

function createGenerator2(n) {
    var _state = 0;
    return {
        next: function () {
            return {
                value: _state,
                done: _state++ >= n
            }
        }
    }
}

var bar = {
    [Symbol.iterator]: function()
    {
        return createGenerator2(this.zmienna)
    },
    zmienna: 5
}

for (var b of bar)
    console.log(b);