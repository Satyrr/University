$(document).ready(function(){

let gameObject = {
	timer:<number> 0,
	timerObject:<any> null,
	timerDOMObject:<any> null,
	checkedNum:<number> 0,
	canCheck:<boolean> false,
	penaltyOffTimeout:<any> null,
	bestTimes:<Array<number>> []
}

function initGame()
{
	// box positions
	for(let i : number=1; i<=9;i++)
	{
		let element : any = $(".box:nth-child(" +i+ ")")
		element.css("top", ((Math.floor((i-1)/3))*165 + (Math.random()*135)) + "px") 
		element.css("left",((i%3)*300 + (Math.random()*270)) + "px")
	}

	// session best scores
	if(localStorage.getItem("bests"))
	{
		let bestTimesString:Array<string> = localStorage.getItem("bests").split(',')
		bestTimesString.forEach(function(v,i) {
	   			gameObject.bestTimes[i] = parseInt(v);
			})
		newBestTimes()
	}
}

let gameStart = function()
{

	$("#central_box").mouseenter(centralBoxEnter)

	$("#central_box").mouseleave(centralBoxLeave)

	$(".box").mouseenter(boxEnter).removeClass("visited_box")

	gameObject.timerDOMObject = $("#timer_time")
	gameObject.timer = 0
	gameObject.checkedNum = 0
	

}

let restart = function()
{
	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.timerDOMObject.html("0")
	gameStart()
}

let gameEnd = function()
{
	$("#central_box").off()

	$("#central_box").off()

	$(".box").off()

	if(gameObject.timerObject)
		clearInterval(gameObject.timerObject)

	gameObject.bestTimes.push(gameObject.timer)
	newBestTimes()
}

function newBestTimes()
{
	gameObject.bestTimes.sort((a, b) => a - b)

	$("#bests").html("")
	for(let time of gameObject.bestTimes)
	{
		let newTime = $("<li></li>")
		newTime.html((time/100.0).toString())

		$("#bests").append(newTime)
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

	if($(this).hasClass("visited_box"))
	{
		gameObject.timer += 100
		$("#timer").css("color", "red")
		if(gameObject.penaltyOffTimeout)
			clearTimeout(gameObject.penaltyOffTimeout)

		gameObject.penaltyOffTimeout = setTimeout(() => { $("#timer").css("color","white") } , 500)
		return;
	}

	if(!gameObject.canCheck)
	{
		return
	}

	$(this).addClass("visited_box")
	gameObject.canCheck = false
	gameObject.checkedNum += 1

	if(gameObject.checkedNum == $(".box").length)
		gameEnd()

}

function timer()
{
	gameObject.timer += 1
	gameObject.timerDOMObject.html(gameObject.timer/100.0)
}

initGame() // boxes position, session best scores
gameStart()

$("#restart_button").click(restart)

});