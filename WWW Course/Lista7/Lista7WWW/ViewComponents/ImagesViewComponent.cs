using Microsoft.AspNetCore.Mvc;

namespace RazorTemplates.ViewComponents
{
    public class ImagesViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string[] model = { "~/images/lew.jpg", "~/images/niedzwiedz.jpg", "~/images/panda.jpg", "~/images/orzel.jpg" };
            return View("Default", model);
        }
    }
}
