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

// start / restart gry
gameObject.gameStart = function()
{

	document.getElementById("central_box")
		.addEventListener("mouseover",gameObject.centralBoxEnter)

	document.getElementById("central_box")
		.addEventListener("mouseout",gameObject.centralBoxLeave)

	if(!gameObject.boxes)
		gameObject.boxes = document.getElementsByClassName("box")

	for(let box of gameObject.boxes)
	{
		box.addEventListener("mouseover",gameObject.boxEnter)
		box.className = "box"
	}

	gameObject.timerDOMObject = document.getElementById("timer_time")
	gameObject.timer = 0
	gameObject.checkedNum = 0
	

}

gameObject.restart = function()
{
	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.timerDOMObject.innerHTML = "0"
	gameObject.gameStart()


}

//koniec gry
gameObject.gameEnd = function()
{
	document.getElementById("central_box")
		.removeEventListener("mouseover",gameObject.centralBoxEnter)

	document.getElementById("central_box")
		.removeEventListener("mouseout",gameObject.centralBoxLeave)

	for(let box of gameObject.boxes)
	{
		box.removeEventListener("mouseover",gameObject.boxEnter)
	}

	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.bestTimes.push(gameObject.timer)
	gameObject.bestTimes.sort()

	document.getElementById("bests").innerHTML = ""
	for(let time of gameObject.bestTimes)
	{
		let newTime = document.createElement('li')
		newTime.innerHTML = time/100.0

		document.getElementById("bests").appendChild(newTime)
	}

}

gameObject.centralBoxEnter = function()
{
	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.canCheck = true
}

gameObject.centralBoxLeave = function()
{
	gameObject.timerObject = setInterval(timer, 10)
}

gameObject.boxEnter = function()
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

gameObject.gameStart()

document.getElementById("restart_button")
	.addEventListener("click",gameObject.restart)