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
                done: fib2 > 100000
            }
        }
    };
}

var _it = fib();
for ( var _result; _result = _it.next(), !_result.done; )
{
    console.log( _result.value );
}

function* fib2() 
{ 
    var fib1 = 0, fib2 = 1;
    while(fib2 < 10000)
    {
        fib2 += fib1
        fib1 = fib2 - fib1
        yield fib2
    }
}

var _it2 = fib2();
for ( var _result; _result = _it2.next(), !_result.done; )
{
    console.log( _result.value );
}

// jak?
for ( var i of fib2() ) {console.log( i );}