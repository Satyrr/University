using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Lista7WWW.Controllers
{
    public class Zad3Controller : Controller
    {
        public IActionResult Index()
        {
            return View("MainPage");
        }

        public IActionResult Gallery()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}