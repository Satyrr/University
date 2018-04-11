var fs = require('fs')
var util = require('util');

//sposob standardowy
/*
fs.readFile( './plik.txt', 'utf-8', 
  function( err, data ) 
  {
    console.log( data );
  }
);*/

function readFileAsync(path, encode)
{
    return new Promise(function(res, rej)
    {
        fs.readFile( path, encode, 
            function( err, data ) 
            {
                if(err)
                    rej(err)
                res(data)
            }
        )
    });
}

var readFileAsync2 = util.promisify(fs.readFile);

readFileAsync('plik.txt', 'utf-8')
.then(function(data) {
    console.log(data + "async + .then()");
});	

readFileAsync2('plik.txt', 'utf-8')
.then(function(data) {
    console.log(data + "async + .then() promisify");
});	

(async function()
{
    var result = await readFileAsync('./plik.txt', 'utf-8')
    console.log(result + "async + async/await");
    result = await readFileAsync2('./plik.txt', 'utf-8')
    console.log(result + "async + async/await promisify");
}
)();