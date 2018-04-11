using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lista7WWW.Models;

namespace Lista7WWW.Controllers
{
    public class Zad4Controller : Controller
    {
        public IActionResult NewCar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewCar(Car car)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Done");
            }

            return View();
        }

        public IActionResult Done()
        {
            return View();
        }
    }
}