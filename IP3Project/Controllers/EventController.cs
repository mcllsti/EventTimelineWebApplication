using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IP3Project.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace IP3Project.Controllers
{
    public class EventController : BaseController
    {
        public IActionResult EventRegister(string Id )
        {

            List<TimelineEventViewModel> list = new List<TimelineEventViewModel>();
            list = GetAllTimelines(list,Id);

            List<TimelineEventsViewModel> model = new List<TimelineEventsViewModel>();
            model = list.First().TimelineEvents;





            return PartialView("_EventRegister", model);

        }

        private List<TimelineEventViewModel> GetAllTimelines(List<TimelineEventViewModel> model, string Id)
        {

            var request = new RestRequest("Timeline/GetAllTimelinesAndEvent"); //setting up the request params
            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var resultsDTO = JsonConvert.DeserializeObject<Timelinez>(response.Content); //Deserializes the results from the response

            foreach(TimelineEventViewModel x in resultsDTO.Timelines.Where(x => x.Id.Equals(Id)))
            {
                model.Add(x);
            }

            return model;
        }









    }
}