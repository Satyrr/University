let mainContent = document.getElementById("content")

let menuDiv = document.createElement('div')
menuDiv.id = "menu"
//menuDiv.style.border = "2px solid blue"

mainContent.appendChild(menuDiv)

let linkRed = document.createElement('a')
linkRed.style.color = "red"
linkRed.innerHTML = "Link czerwony "
linkRed.href="#"
linkRed.addEventListener("click", () => changeBorderColor("red"))

let linkBlack = document.createElement('a')
linkBlack.style.color = "black"
linkBlack.innerHTML = "Link czarny"
linkBlack.href = "#"
linkBlack.addEventListener("click", () => changeBorderColor("black"))

let linkBrown = document.createElement('a')
linkBrown.style.color = "brown"
linkBrown.innerHTML = "Link brązowy"
linkBrown.href="#"
linkBrown.addEventListener("click", () => changeBorderColor("brown"))


let br = document.createElement('br')
menuDiv.append(linkRed, br, linkBlack, br.cloneNode(), linkBrown)

function changeBorderColor(color)
{
	menuDiv.style.border = "5px solid " + color
}