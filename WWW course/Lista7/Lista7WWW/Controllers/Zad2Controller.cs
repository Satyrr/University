using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lista7WWW.Services;

namespace Lista7WWW.Controllers
{
    public class Zad2Controller : Controller
    {
        const int pageSize = 3;
        private IPersonRepository _personRepository;

        public Zad2Controller(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IActionResult Index(int? pageNr)
        {
            if (pageNr == null)
                pageNr = 1;
 
            int personCount = _personRepository.GetAll().Count();
            var model = _personRepository.GetAll().Skip(pageSize * (pageNr.Value-1)).Take(pageSize);

            ViewBag.pageNumber = (int)Math.Ceiling((float)personCount / pageSize);

            ViewBag.currentPage = pageNr;
            return View(model);
        }
    }
}