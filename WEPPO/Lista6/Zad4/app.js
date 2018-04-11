var http = require('http');
var express = require('express');
var path = require('path')


var app = express();

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

app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, './views'));
app.use( express.urlencoded({extended:true}) ) ;

app.get('/wyswietldokumenty/:userid', authorize, (req, res) =>
{
    //Dodatkowa walidacja:

    var user = users.filter((u) => {return u.id == req.params.userid} )
    res.render("wyswietldokumenty", { user:user[0]})
})



// tu uruchamiamy serwer
var server = http.createServer(app).listen(3000);

console.log( 'serwer started' );