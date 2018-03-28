using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IP3Project.Models;
using RestSharp;
using Newtonsoft.Json;


namespace IP3Project.Controllers
{
    public class HomeController : BaseController
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
