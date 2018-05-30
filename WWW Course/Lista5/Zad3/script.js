$(document).ready(function () {
    var gameObject = {
        timer: 0,
        timerObject: null,
        timerDOMObject: null,
        checkedNum: 0,
        canCheck: false,
        penaltyOffTimeout: null,
        bestTimes: []
    };
    function initGame() {
        for (var i = 1; i <= 9; i++) {
            var element = $(".box:nth-child(" + i + ")");
            element.css("top", ((Math.floor((i - 1) / 3)) * 165 + (Math.random() * 135)) + "px");
            element.css("left", ((i % 3) * 300 + (Math.random() * 270)) + "px");
        }
        if (localStorage.getItem("bests")) {
            var bestTimesString = localStorage.getItem("bests").split(',');
            bestTimesString.forEach(function (v, i) {
                gameObject.bestTimes[i] = parseInt(v);
            });
            newBestTimes();
        }
    }
    var gameStart = function () {
        $("#central_box").mouseenter(centralBoxEnter);
        $("#central_box").mouseleave(centralBoxLeave);
        $(".box").mouseenter(boxEnter).removeClass("visited_box");
        gameObject.timerDOMObject = $("#timer_time");
        gameObject.timer = 0;
        gameObject.checkedNum = 0;
    };
    var restart = function () {
        if (gameObject.timerObject)
            clearInterval(gameObject.timerObject);
        gameObject.timerDOMObject.html("0");
        gameStart();
    };
    var gameEnd = function () {
        $("#central_box").off();
        $("#central_box").off();
        $(".box").off();
        if (gameObject.timerObject)
            clearInterval(gameObject.timerObject);
        gameObject.bestTimes.push(gameObject.timer);
        newBestTimes();
    };
    function newBestTimes() {
        gameObject.bestTimes.sort(function (a, b) { return a - b; });
        $("#bests").html("");
        for (var _i = 0, _a = gameObject.bestTimes; _i < _a.length; _i++) {
            var time = _a[_i];
            var newTime = $("<li></li>");
            newTime.html((time / 100.0).toString());
            $("#bests").append(newTime);
        }
        localStorage.setItem("bests", gameObject.bestTimes.toString());
    }
    var centralBoxEnter = function () {
        if (gameObject.timerObject)
            clearInterval(gameObject.timerObject);
        gameObject.canCheck = true;
    };
    var centralBoxLeave = function () {
        gameObject.timerObject = setInterval(timer, 10);
    };
    var boxEnter = function () {
        if ($(this).hasClass("visited_box")) {
            gameObject.timer += 100;
            $("#timer").css("color", "red");
            if (gameObject.penaltyOffTimeout)
                clearTimeout(gameObject.penaltyOffTimeout);
            gameObject.penaltyOffTimeout = setTimeout(function () { $("#timer").css("color", "white"); }, 500);
            return;
        }
        if (!gameObject.canCheck) {
            return;
        }
        $(this).addClass("visited_box");
        gameObject.canCheck = false;
        gameObject.checkedNum += 1;
        if (gameObject.checkedNum == $(".box").length)
            gameEnd();
    };
    function timer() {
        gameObject.timer += 1;
        gameObject.timerDOMObject.html(gameObject.timer / 100.0);
    }
    initGame();
    gameStart();
    $("#restart_button").click(restart);
});
//# sourceMappingURL=script.js.map