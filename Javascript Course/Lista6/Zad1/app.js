var http = require('http');
var express = require('express');
var multer  = require('multer')
var path = require('path')
var upload = multer({ dest: 'uploads/' })

//if( c ) { }
//var obj = { }
//if ( obj.c ) { }


var app = express();

app.set('view engine', 'ejs');
//app.set('views', './Zad1/views');
app.set('views', path.join(__dirname, './views'));
app.use( express.urlencoded({extended:true}) ) ;

// tu dodajemy middleware
app.get('/plik',(req, res) =>
{
    res.render("plik")
})

app.post('/plik',upload.array('plik', 5),(req, res) =>
{
    res.render("plik", { files:req.files } )
})


// tu uruchamiamy serwer
var server = http.createServer(app).listen(3000);

console.log( 'serwer started' );