fs = require('fs')
fs.readFile( './plik.txt', 'utf-8', 
  function( err, data ) 
  {
    console.log( data );
  }
);

var Person = function(name, age)
{
	this.name = name
	this.age = age
}

var p = new Person("aaa",15)

console.log(Object.getPrototypeOf(p))
console.log(Object.getPrototypeOf(Object.getPrototypeOf(p)))