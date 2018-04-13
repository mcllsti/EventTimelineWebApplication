using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IP3Project.Models;


namespace IP3Project.Controllers
{
    /// <summary>
    ///Author: Team16, Microsoft
    ///Date: Trimester 2, 2018 
    ///Version: 1.0 
    /// 
    /// Auto-Generated controller by Microsoft MVC that returns Index, About, Help and Error pages.
    /// This code has remained unedited. 
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Welcome";

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "About Page";

            return View();
        }

        public IActionResult Help()
        {
            ViewData["Message"] = "Help Page";

            return View();
        }

        public IActionResult PageNotFound()
        {
            ViewData["Message"] = "Page not found!";
            return View("Error", (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
