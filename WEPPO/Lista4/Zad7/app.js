const readline = require('readline');
const fs = require('fs')

var ipDictionary = { }

const rl = readline.createInterface({
    input: fs.createReadStream('access_log.txt'),
    crlfDelay: Infinity
  });
  
  rl.on('line', (log_line) => {
    var ip = log_line.split(' - - ')[0]
    if(ipDictionary[ip])
        ipDictionary[ip] += 1
    else
        ipDictionary[ip]  = 1
  });

rl.on('close', () =>
{
    
    var res = Object.keys(ipDictionary).sort(function(a,b){return ipDictionary[b]-ipDictionary[a]})
    console.log("Najczestsze ip: " + res[0] + "( " + ipDictionary[res[0]] + " razy)")
  })