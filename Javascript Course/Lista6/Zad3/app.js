var http = require('http');
var express = require('express');
var path = require('path')
var cookieParser = require('cookie-parser')
var session = require('express-session')
var FileStore = require('session-file-store')(session);

var app = express();

app.set('view engine', 'ejs');
//app.set('views', './Zad1/views');
app.set('views', path.join(__dirname, './views'));
app.use( express.urlencoded({extended:true}) ) ;
app.use(cookieParser())
app.use(session({resave:true, saveUninitialized: true, store: new FileStore(), secret: 'qewhiugriasgy' }));



// ciastka
app.get('/getcookie',(req, res) =>
{
    res.cookie("testcookie", "testval")
    res.redirect("/checkcookie")
})

app.get('/checkcookie',(req, res) =>
{
    res.render("checkcookie", { cookies : req.cookies })
})

app.get('/deletecookie',(req, res) =>
{
    //res.clearCookie('testcookie')
    res.cookie("testcookie", "a", {maxAge: -1});
    res.redirect("/checkcookie")
})

// sesje
app.get('/setsession/:name',(req, res) =>
{
    req.session.name = req.params.name
    res.redirect("/checksession")
})

app.get('/checksession',(req, res) =>
{
    res.render("checksession", { session : req.session })
})

app.get('/deletesession',(req, res) =>
{
    req.session.destroy()
    res.redirect("/checksession")
})


// tu uruchamiamy serwer
var server = http.createServer(app).listen(3000);

console.log( 'serwer started' );