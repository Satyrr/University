var http = require('http');
var express = require('express');
var path = require('path')
var cookieParser = require('cookie-parser')

var app = express();

app.set('view engine', 'ejs');
//app.set('views', path.join(__dirname, './views'));
app.set('views', 'Zad4/views');

app.use( express.urlencoded({extended:true}) ) ;
app.use(cookieParser())

// tu dodajemy middleware
app.get('/',(req, res) =>
{
    res.render("formularz")
})

app.post('/',(req, res) =>
{
    for(var i=1;i<=10;i++)
        if(!req.body["exercise" + i])
            req.body["exercise" + i] = 0
            
    var required = ["firstname", "surname", "course"]
    if(required.some( (v,idx,arr) => { return !req.body[v] } ))
    {
        req.body.error = "Uzupelnij wszystkie pola!!"
        res.render("formularz", req.body)
    }  
    else
    {
        res.cookie("formularz", JSON.stringify(req.body))
        res.redirect("/print")
    }
})

app.get('/print',(req, res) =>
{
    console.log(req.cookies)
    if(req.cookies["formularz"])
        res.render("wydruk", JSON.parse(req.cookies["formularz"]))
    else
        res.redirect("/")
})


// tu uruchamiamy serwer
var server = http.createServer(app).listen(3000);

console.log( 'serwer started' );