$(document).ready(function () {
    var slider_current_idx = 0;
    var slide_height = parseInt($("li.slide").first().css("height"));
    var slider_cnt = $("li.slide").length;
    function changeSlide() {
        var prev_idx = slider_current_idx;
        slider_current_idx = (slider_current_idx + 1) % slider_cnt;
        switch (prev_idx % 2) {
            case 0:
                $("ul.slider").children().eq(prev_idx).fadeOut(700, function () { return $("ul.slider").children().eq(slider_current_idx).fadeIn(700); });
                break;
            case 1:
                $("ul.slider").children().eq(prev_idx).slideUp(700, function () { return $("ul.slider").children().eq(slider_current_idx).slideDown(700); });
                break;
        }
    }
    changeSlide();
    var sliderFunc = setInterval(changeSlide, 3000);
    $("div#sliderdiv").mouseenter(function () { clearInterval(sliderFunc); });
    $("div#sliderdiv").mouseleave(function () { return sliderFunc = setInterval(changeSlide, 3000); });
});
//# sourceMappingURL=script.js.map