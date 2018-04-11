function fib()
{
    var fib1 = 0, fib2 = 1;
    return { 
        next: function () 
        {
            fib2 += fib1
            fib1 = fib2 - fib1
            return {
                value: fib2,
                done: false
            }
        }
    };
}

function* fib2() 
{ 
    var fib1 = 0, fib2 = 1;
    while(1)
    {
        fib2 += fib1
        fib1 = fib2 - fib1
        yield fib2
    }
}


function* take(it, top)
{
    var i = 0
    var _it = it()
    for(var _result; _result = _it.next(), !_result.done && i < top;)
    {
        yield _result.value
        i+=1
    }
}

for(var f of take(fib, 10))
{
    console.log(f)
}