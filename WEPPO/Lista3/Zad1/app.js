var foo = {
    zmienna: 5,
    funkcja: function(a)
    {
        return a*2;
    },
    get akcesor()
    {
        if(this.zmienna > 10)
            return 0;
        else
            return 1;
    },
    set akcesor(value)
    {
        this.zmienna = value;
    }
}

foo.akcesor = 15
console.log(foo.zmienna)

foo.nowa_zmienna = 10;
foo.nowa_metoda = function(argument)
{
    return argument*3;
}

Object.defineProperty(foo,
     'nowy_akcesor',
    { get: function(){ return this.zmienna + 2;},
      set: function(value){ this.zmienna = value + 2 ;}
    })

console.log(foo.nowy_akcesor);
foo.nowy_akcesor = 10;
console.log(foo.zmienna)