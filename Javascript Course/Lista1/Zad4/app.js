function fibiter(n)
{
    let fib1 = 1;
    let fib2 = 1;

    for(var i = 2; i < n; i++)
    {
        var temp = fib1;
        fib1 = fib2;
        fib2 = fib2 + temp;
    }

    return fib2;
}

function fibrec(n)
{
    if(n == 1 || n == 2)
      return 1;

    return fibrec(n-1) + fibrec(n-2);
}



/*
for(var n = 20; n < 40; n++)
{
    console.log('n = ' + n)
    console.time('rek')
    fibrec(n)
    var rek_time = console.timeEnd('rek')

    console.time('iter')
    fibiter(n)
    var iter_time = console.timeEnd('iter')
}
*/
console.log('n\t' + 'rekurencyjna(ms)\t' + 'iteracyjna(ms)')
for(var n = 30; n < 50; n++)
{
    var start = new Date();
    fibrec(n)
    var rek_time = new Date() - start;

    start = new Date();
    fibiter(n)
    var iter_time = new Date() - start;

    console.log(n + '\t' +  rek_time + '\t\t\t' + iter_time)
}

