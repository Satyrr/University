var http = require('http');
var express = require('express');
var bodyParser = require('body-parser');
var cookieParser = require('cookie-parser');
var path = require('path')

var app = express();

app.use(bodyParser.urlencoded({ extended: true }));
app.use(cookieParser('sgs90890s8g90as8rg90as8g9r8a0srg8'));

app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, './views'));

// wymaga logowania
app.get( '/', authorize, (req, res) => {
    res.render('app', { user : req.user } );
});

app.get('/dokumenty/:username', authorize, (req, res) =>
{
    //Dodatkowa walidacja:
    var user = users.find((u) => {return u.name == req.params.username && u.name == req.user} )

    res.render("wyswietldokumenty", { user:user })
})

app.get( '/logout', authorize, (req, res) => {
    res.cookie('user', '', { maxAge: -1 } );
    res.redirect('/')
});

// strona logowania
app.get( '/login', (req, res) => {
    res.render('login');
});

app.post( '/login', (req, res) => {
    var username = req.body.txtUser;
    var pwd = req.body.txtPwd;

    if ( username == pwd ) {
        // wydanie ciastka
        res.cookie('user', username);
        // przekierowanie
        var returnUrl = req.query.returnUrl;
        res.redirect(returnUrl);
    } else {
        res.render( 'login', { message : "Zła nazwa logowania lub hasło" } );
    }
});

// middleware autentykacji
function authorize(req, res, next) {
    if ( req.cookies.user ) {
        req.user = req.cookies.user;
        next();
    } else {
        res.redirect('/login?returnUrl='+req.url);
    }
}

http.createServer(app).listen(3000);
console.log( 'serwer działa, nawiguj do http://localhost:3000' );

var user1 = {
    name:"user1",
    id:1,
    documents: [
        {id:1, content:"Tajny dokument1 usera1"},
        {id:2, content:"Tajny dokument2 usera1"},
    ]
}
var user2 = {
    name:"user2",
    id:2,
    documents: [
        {id:3, content:"Tajny dokument1 usera2"},
        {id:4, content:"Tajny dokument2 usera2"},
    ]
}
var users = [user1, user2]

a = 1;
var obj = {}
if(a)