var Foo = function()
{
}

//
Foo.prototype = (function()
{
    function Qux() { return "Tajemna wiadomosc"}
    return {
        Bar:function()
        {
            var message = "To jest publiczna metoda. Wartosc zwrocona z metody prywatnej Qux = " + Qux()
            console.log(message)
        },
        Qux2:function()
        {
            console.log("QUX2");
        }
    }
})();

Foo.prototype.aaa = function()
{ 
    Qux();
}




foo = new Foo()

foo.Bar()
foo.aaa()


//wielowątkowe vs jednowatkowe
//async await w C#
//eventy nie zatrzymujace kodu
// 