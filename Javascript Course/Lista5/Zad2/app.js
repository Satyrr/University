var https = require('https');
var fs = require('fs')
var options = {
    pfx: fs.readFileSync('./Zad2/bar.pfx'),
    passphrase: 'bar'
};

https.createServer(options, (req, res) => {

    res.setHeader('Content-type', 'text/html; charset=utf-8');
    res.write('Witamy w node.js, polskie znaki ąłżńóź');
    res.end();

})
.listen(3000);
