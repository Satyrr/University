let gameObject = {
	timer: 0,
	timerObject: null,
	timerDOMObject: null,
	checkedNum: 0,
	canCheck: false,
	boxes: null,
	penaltyOffTimeout: null,
	bestTimes: []
}

function init_game()
{
	// box positions
	for(let i=1; i<=9;i++)
	{
		let element = document.querySelector(".box:nth-child(" +i+ ")")
		element.style.top = ((Math.floor((i-1)/3))*165 + (Math.random()*135)) + "px"
		element.style.left = ((i%3)*300 + (Math.random()*270)) + "px"
	}

	// session best scores
	if(localStorage.getItem("bests"))
	{
		gameObject.bestTimes = localStorage.getItem("bests").split(',')
		gameObject.bestTimes.forEach(function(v,i,arr) {
	   			arr[i] = parseInt(v);
			})
		newBestTimes()
	}
}

gameObject.gameStart = function()
{

	document.getElementById("central_box")
		.addEventListener("mouseover",centralBoxEnter)

	document.getElementById("central_box")
		.addEventListener("mouseout",centralBoxLeave)

	if(!gameObject.boxes)
		gameObject.boxes = document.getElementsByClassName("box")

	for(let box of gameObject.boxes)
	{
		box.addEventListener("mouseover",boxEnter)
		box.className = "box"
	}

	gameObject.timerDOMObject = document.getElementById("timer_time")
	gameObject.timer = 0
	gameObject.checkedNum = 0
	

}

var restart = function()
{
	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.timerDOMObject.innerHTML = "0"
	gameObject.gameStart()


}

gameObject.gameEnd = function()
{
	document.getElementById("central_box")
		.removeEventListener("mouseover",centralBoxEnter)

	document.getElementById("central_box")
		.removeEventListener("mouseout",centralBoxLeave)

	for(let box of gameObject.boxes)
	{
		box.removeEventListener("mouseover",boxEnter)
	}

	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.bestTimes.push(gameObject.timer)
	newBestTimes()

}

function newBestTimes()
{
	gameObject.bestTimes.sort((a, b) => a - b)

	document.getElementById("bests").innerHTML = ""
	for(let time of gameObject.bestTimes)
	{
		let newTime = document.createElement('li')
		newTime.innerHTML = time/100.0

		document.getElementById("bests").appendChild(newTime)
	}
	localStorage.setItem("bests", gameObject.bestTimes.toString())
}

var centralBoxEnter = function()
{
	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.canCheck = true
}

var centralBoxLeave = function()
{
	gameObject.timerObject = setInterval(timer, 10)
}

var boxEnter = function()
{

	if(this.classList.contains("visited_box"))
	{
		gameObject.timer += 100
		document.getElementById("timer").style.color = 'red';
		if(gameObject.penaltyOffTimeout)
			clearTimeout(gameObject.penaltyOffTimeout)

		gameObject.penaltyOffTimeout = setTimeout(() => { document.getElementById("timer").style.color = 'white' } , 500)
		return;
	}

	if(!gameObject.canCheck)
	{
		return
	}

	this.className += " visited_box"
	gameObject.canCheck = false
	gameObject.checkedNum += 1

	if(gameObject.checkedNum == gameObject.boxes.length)
		gameObject.gameEnd()

}

function timer()
{
	gameObject.timer += 1
	gameObject.timerDOMObject.innerHTML = gameObject.timer/100.0
}

init_game() // boxes position, session best scores
gameObject.gameStart()

document.getElementById("restart_button")
	.addEventListener("click",restart)