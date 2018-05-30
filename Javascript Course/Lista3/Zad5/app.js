function sum(...args)
{
    var sum = 0;
    for(s of args)
    {
        sum += s;
    }

    return sum;
}

console.log(sum(1,2,3,5,7,4))
console.log(sum())
console.log(sum(1,2))