var http = require('http');
var express = require('express');
var path = require('path')

var app = express();

app.set('view engine', 'ejs');
//app.set('views', './Zad1/views');
app.set('views', path.join(__dirname, './views'));
app.use( express.urlencoded({extended:true}) ) ;

// tu dodajemy middleware
app.get('/:id',(req, res) =>
{
    res.render("index", { id : req.params.id})
})



// tu uruchamiamy serwer
var server = http.createServer(app).listen(3000);

console.log( 'serwer started' );