$(document).ready(function(){

(function()
{
   var isLarge = $(this).scrollTop() > document.documentElement.clientHeight/5? true:false;

   function headerSliding()
   {
   		if($(this).scrollTop() > document.documentElement.clientHeight/5 && isLarge)
   		{
   			$(".navbar").eq(0).stop().animate({"height": '10vh', opacity:0.5, queue: false}, 200)
   				.removeClass("bg-primary").addClass("bg-dark");
   			isLarge = false;
   		}
   		else if($(this).scrollTop() <= document.documentElement.clientHeight/5 && !isLarge)
   		{
   			$(".navbar").eq(0).stop().animate({"height": '20vh', opacity:1.0, queue: false}, 200)
   				.addClass("bg-primary").removeClass("bg-dark");
   			isLarge = true;
   		}
   }
   headerSliding();
   $(document).scroll(headerSliding)
})();



});

