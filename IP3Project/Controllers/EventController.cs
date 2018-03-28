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

        [HttpGet]
        public ActionResult TimelineView(string id)
        {
            Timeline model = new Timeline(id);
            model = GetTimeline(model);
            model.TimelineEvents = GetEvents(id).Events;


            return View(model);
        }

        public IActionResult EventRegister(string Id )
        {

            EventList model = GetEvents(Id);

            return PartialView("_EventRegister", model);

        }

        private EventList GetEvents(string Id)
        {

            var request = new RestRequest("Timeline/GetAllTimelinesAndEvent"); //setting up the request params
            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var resultsDTO = JsonConvert.DeserializeObject<TimelineList>(response.Content); //Deserializes the results from the response
            EventList model = new EventList();

            List<Event> EventsList = new List<Event>();

            foreach(Timeline x in resultsDTO.Timelines.Where(x => x.Id.Equals(Id)))
            {
                model.Events = x.TimelineEvents;
                
            }

            return model;
        }

        private Timeline GetTimeline(Timeline model)
        {

            var request = new RestRequest("Timeline/GetTimeline"); //setting up the request params
            request.AddHeader("TimelineId", model.Id);

            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            model = JsonConvert.DeserializeObject<Timeline>(response.Content); //Deserializes the results from the response

            return model;
        }


        private AttachmentList GetAttachments(string eventId)
        {

            var request = new RestRequest("TimelineEventAttachment/GetAttachmets"); //setting up the request params
            request.AddHeader("TimelineEventId", eventId);

            IRestResponse response = API.GetRequest(request);
            
            AttachmentList model = new AttachmentList();

            model = JsonConvert.DeserializeObject<AttachmentList>(response.Content);

            //List<AttachmentViewModel> AllAttachements = new List<AttachmentViewModel>();

            return (model);
        }








    }
}