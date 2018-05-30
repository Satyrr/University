/*function createFs(n) {
    // tworzy tablicę n funkcji
    var fs = [];
    // i-ta funkcja z tablicy ma zwrócić i
    for ( let i=0; i<n; i++ ) 
    {
        fs[i] = function() { return i; };
    };
    return fs;
}*/

/*
function createFs(n) {
    // tworzy tablicę n funkcji
    var fs = [];
    // i-ta funkcja z tablicy ma zwrócić i
    for ( var i=0; i<n; i++ ) 
    {
        (function()
        {
            var i2 = i;
            fs[i] = function() { return i2; };
        })();
    };
    return fs;
}
var myfs = createFs(10);
*/

function createFs(n) {
    // tworzy tablicę n funkcji
    var fs = [];
    // i-ta funkcja z tablicy ma zwrócić i
    for ( var i=0; i<n; i++ ) 
    {
        (function(n)
        {
            fs[i] = function() { return n; };
        })(i);
    };
    return fs;
}
var myfs = createFs(10);

console.log( myfs[0]() );// zerowa funkcja miała zwrócić 0
console.log( myfs[2]() );// druga miała zwrócić 2
console.log( myfs[7]() );