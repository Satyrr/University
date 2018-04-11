function forEach( a, f)
{
    for(var i = 0; i < a.length; i++)
    {
        f(a[i]);
    }
}

function map( a, f)
{
    b = []
    for(var i = 0; i < a.length; i++)
    {
        b[i] = f(a[i]);
    }
    return b;
}

function filter( a, f)
{
    b = []
    for(var i = 0; i < a.length; i++)
    {
        if(f(a[i]) == true)
            b.push(a[i]);
    }
    return b;
}


var ar = [1,5,10,12,15,17];
console.log(ar);

forEach(ar, _ => console.log(_));

var squares = map(ar, _ => _*_);
console.log(squares);

var less_then = filter(ar, _ => _ < 11);
console.log(less_then);

forEach(ar, function(n)
{
    console.log(Math.sqrt(n));
})

var logs = map(ar, function(n)
{
    return Math.log10(n);
});

var even = filter(ar, function(n)
{
    return (n % 2) == 0;
});
console.log(logs);
console.log(even)

var arr2 = [123,2,345,5,500,1000]
console.log(arr2.sort())