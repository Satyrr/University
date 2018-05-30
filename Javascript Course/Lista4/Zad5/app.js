var name = ""

process.stdin.setEncoding('utf8');


//dlaczego readable nie powoduje przejscia we flowing mode
process.stdin.on('data', () => {
  /*const chunk = process.stdin.read();
  if (chunk !== null) {
    name = chunk
    process.stdin.emit('end')
  }
    */
});



process.stdin.on('end', () => {
  console.log("Witaj " + name)
});