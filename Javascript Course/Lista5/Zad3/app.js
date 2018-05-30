var http = require('http');
var fs = require('fs')
//
http.createServer( (req, res) => {

    res.setHeader('Content-type', 'text/html; charset=utf-8');
    res.setHeader('Content-Disposition', 'attachment')
    // czy write powoduje wyslanie naglowka?
    // certyfikaty
    

    var fileSteam = fs.createReadStream('./Zad3/plik')
    fileSteam.pipe(res)
    //res.end();

})
.listen(3000);