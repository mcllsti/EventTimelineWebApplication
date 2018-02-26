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


            var resultsDTO = JsonConvert.DeserializeObject<List<Timeline>>(response.Content);

            TimelineList model = new TimelineList();
            foreach (Timeline x in resultsDTO)
            {
                model.timelines.Add(new TimelineViewModel(x.Title,x.CreationTimeStamp,x.Id));
            }

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
