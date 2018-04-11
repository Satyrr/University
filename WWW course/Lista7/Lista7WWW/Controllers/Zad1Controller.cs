using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lista7WWW.Controllers
{
    public class Zad1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string Przyklad1()
        {
            return "Zwykły napis";
        }

        public async Task<FileContentResult> Przyklad2()
        {
            string path = Path.GetFullPath("./wwwroot/images/pies.jpg");
            
            return new FileContentResult(await System.IO.File.ReadAllBytesAsync(path), "image/jpeg");
        }

        public JsonResult Przyklad3()
        {
            return new JsonResult(new { wartosc = 1, wartosc2 = 2});
        }

        public IActionResult Przyklad4()
        {
            return Redirect("https://onet.pl");
        }
    }
}