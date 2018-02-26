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
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            var client = new RestClient("https://gcu.ideagen-development.com/Timeline/GetTimelines");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "a8183267-1cc4-4c7b-ac1d-3d4479d10197");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("AuthToken", "3dbd6fb2-caa0-4083-b286-6544baf2a248");
            request.AddHeader("TenantId", "Team16");
            IRestResponse response = client.Execute(request);
            List<Timeline> customerDto = JsonConvert.DeserializeObject<List<Timeline>>(response.Content);



            return View();
        }

        public IActionResult About()
        {
            

            return View();
        }

        public IActionResult Help()
        {
            ViewData["Message"] = "Help Page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
