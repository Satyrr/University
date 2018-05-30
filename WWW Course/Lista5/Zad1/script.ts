$(document).ready(function(){

let slider_current_idx:number = 0
let slide_height:number = parseInt($("li.slide").first().css("height"))
let slider_cnt:number = $("li.slide").length

function changeSlide()
{
	let prev_idx:number = slider_current_idx
	slider_current_idx = (slider_current_idx + 1) % slider_cnt
	
	switch(prev_idx % 2)
	{
		case 0:
			$("ul.slider").children().eq(prev_idx).fadeOut(700,
				() => $("ul.slider").children().eq(slider_current_idx).fadeIn(700))
			break;
		case 1:
			$("ul.slider").children().eq(prev_idx).slideUp(700,
				() => $("ul.slider").children().eq(slider_current_idx).slideDown(700))
			break;
	}
}

changeSlide()
let sliderFunc:any = setInterval(changeSlide, 3000)


$("div#sliderdiv").mouseenter(() => {clearInterval(sliderFunc)})
$("div#sliderdiv").mouseleave(() => sliderFunc = setInterval(changeSlide, 3000))
});


