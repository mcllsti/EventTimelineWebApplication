using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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


        #region Update

        #region Description
        [HttpGet]
        public PartialViewResult EditDescription(string Id, string Description)
        {
            EditDescription model = new EditDescription(Id, Description);
            return PartialView("_EditDescription", model);

        }

        [HttpPost]
        public IActionResult EditDescription(EditDescription model)
        {
            string succsess = EditDescriptionCall(model);
            if (!succsess.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private string EditDescriptionCall(EditDescription model)
        {
            var request = new RestRequest("TimelineEvent/EditDescription");
            return API.PutRequest(request, model);
        }

        #endregion

        #region Date
        [HttpGet]
        public PartialViewResult EditDate(string Id, string Date)
        {
            EditDate model = new EditDate(Id,Date);
            return PartialView("_EditDate", model);

        }

        [HttpPost]
        public IActionResult EditDate(EditDate model)
        {
            string succsess = EditDateCall(model);
            if (!succsess.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private string EditDateCall(EditDate model)
        {
            model.EventDateTime.ToString();
            CultureInfo ukCulture = new CultureInfo("en-GB");
            try
            {
                model.EventDateTime = DateTime.Parse(model.EventDateTime, ukCulture.DateTimeFormat).Ticks.ToString();
            }
            catch
            {
                return "error";
            }

            var request = new RestRequest("TimelineEvent/EditEventDateTime");
            return API.PutRequest(request, model);
        }

        #endregion

        #region Location
        [HttpGet]
        public PartialViewResult EditLocation(string Id, string Location)
        {
            EditLocation model = new EditLocation(Id, Location);
            return PartialView("_EditLocation", model);

        }

        [HttpPost]
        public IActionResult EditLocation(EditLocation model)
        {
            string succsess = EditLocationCall(model);
            if(!succsess.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private string EditLocationCall(EditLocation model)
        {
            var request = new RestRequest("TimelineEvent/EditLocation");
            return API.PutRequest(request, model);
        }

        public IActionResult RemoveLocation(string Id)
        {
            EditLocation model = new EditLocation(Id, null);
            var request = new RestRequest("TimelineEvent/EditLocation");
            if(!(API.PutRequest(request, model)).Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!
        }



        #endregion

        #region Title
        [HttpGet]
        public PartialViewResult EditTitle(string Id, string Title)
        {
            EditTitle model = new EditTitle(Id, Title);
            return PartialView("_EditTitle", model);

        }

        [HttpPost]
        public IActionResult EditTitle(EditTitle model)
        {
            string succsess = EditTitleCall(model);
            if (!succsess.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private string EditTitleCall(EditTitle model)
        {
            var request = new RestRequest("TimelineEvent/EditTitle");
            return API.PutRequest(request, model);
        }

        #endregion


        #endregion



        #region Create
        [HttpGet]
        public PartialViewResult CreateEvent(string Id)
        {
            ViewData["Title"] = "Create a Event";
            ViewData["Action"] = "CreateEvent";
            CreateEventView model = new CreateEventView(Id);
            return PartialView("_CreateEvent", model);

        }

        [HttpPost]
        public IActionResult CreateEvent(CreateEventView model)
        {
            string successful;
            successful = Create(model);
            if(!successful.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            successful = Link(model);

            if (!successful.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }

            return RedirectToAction("TimelineView", new { id = model.TimelineId }); //returns to the Index!

        }
        #endregion

        #region Delete
        public IActionResult DeleteEvent(string Id)
        {

            string TimelineId = GetTimelineID(Id);
            string success;
            success = UnlinkEvent(TimelineId, Id);
            if (!success.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            success = Delete(Id);
            if(!success.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }

            return RedirectToAction("TimelineView", new { id = TimelineId }); //returns to the Index!
        }
        #endregion

        #region Read

        [HttpGet]
        public ActionResult TimelineView(string id)
        {
            Timeline model = new Timeline(id);
            model = GetTimeline(model);

            if(model == null)
            {
                
            }
            try
            {
                model.TimelineEvents = GetEvents(id).Events;
            }
            catch
            {
                return RedirectToAction("APIError");
            }


            return View(model);
        }

        public IActionResult EventRegister(string Id)
        {

            EventList model = GetEvents(Id);

            return PartialView("_EventRegister", model);

        }


        public IActionResult DisplayEvent(string Id)
        {
            Event EventToDisplay = new Event();
            EventToDisplay = GetEvent(Id);
            if(EventToDisplay == null)
            {
                return RedirectToAction("APIError");
            }

            return PartialView("_EventView", EventToDisplay);
        }
        #endregion

        #region Utility

        private string Link(CreateEventView EventToLink)
        {

            TimelineEventLink LinkToCreate = new TimelineEventLink(EventToLink.TimelineId, EventToLink.TimelineEventId);

            var request = new RestRequest("/Timeline/LinkEvent");
            string success = API.PutRequest(request, LinkToCreate);

            return success;

        }

        private Timeline GetTimeline(Timeline model)
        {

            var request = new RestRequest("Timeline/GetTimeline"); //setting up the request params
            request.AddHeader("TimelineId", model.Id);

            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response
            try
            {
                model = JsonConvert.DeserializeObject<Timeline>(response.Content); //Deserializes the results from the response
            }
            catch
            {
                model = null;
            }


            return model;
        }



        private EventList GetEvents(string Id)
        {
            EventList model = new EventList();
            TimelineList resultsDTO = new TimelineList();
            resultsDTO = GetSystemData();

            if(resultsDTO == null)
            {
                model = null;
                return model;
            }

            List<Event> EventsList = new List<Event>();

            foreach (Timeline x in resultsDTO.Timelines.Where(x => x.Id.Equals(Id)))
            {
                model.Events = x.TimelineEvents;
            }

            return model;
        }

        private TimelineList GetSystemData()
        {
            var request = new RestRequest("Timeline/GetAllTimelinesAndEvent"); //setting up the request params
            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response
            
            var resultsDTO = JsonConvert.DeserializeObject<TimelineList>(response.Content); //Deserializes the results from the response

            if (!response.StatusDescription.Equals("OK")) { resultsDTO = null; }

            return resultsDTO;

        }

        private string GetTimelineID(string Id)
        {
            TimelineList resultsDTO = new TimelineList();
            resultsDTO = GetSystemData();

            Timeline timeline = new Timeline();
            timeline = resultsDTO.Timelines.Find(x => x.TimelineEvents.Exists(y => y.Id == Id));

            return timeline.Id;
        }

        private string Delete(string Id)
        {
            DeleteEventViewModel delete = new DeleteEventViewModel(Id);
            var request = new RestRequest("TimelineEvent/Delete");

            return API.PutRequest(request, delete);

        }

        private Event GetEvent(string Id)
        {

            var request = new RestRequest("TimelineEvent/GetTimelineEvent"); //setting up the request params
            request.AddHeader("TimelineEventId", Id);

            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var model = JsonConvert.DeserializeObject<Event>(response.Content); //Deserializes the results from the response
            if(!response.StatusDescription.Equals("OK"))
            {
                model = null;
                return model;
            }
            List<Attachment> Attachments = new List<Attachment>();
            model.Attachments = GetEventAttachments(Id);

            return model;

        }

        /// <summary>
        /// Gets and returns list of attachments on a single event
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<Attachment> GetEventAttachments(string Id)
        {
            var request = new RestRequest("TimelineEventAttachment/GetAttachments"); //setting up the request params
            request.AddHeader("TimelineEventId", Id);

            IRestResponse response = API.GetRequest(request);

            var model = JsonConvert.DeserializeObject<List<Attachment>>(response.Content);

            return model;
        }

        private string UnlinkEvent(string TimelineId, string EventId)
        {
            var request = new RestRequest("Timeline/UnlinkEvent");
            TimelineEventLink Unlink = new TimelineEventLink(TimelineId, EventId);

            return API.PutRequest(request, Unlink);

        }

        private string Create(CreateEventView EventToCreate)
        {
            var request = new RestRequest("TimelineEvent/Create");

            EventToCreate.TimelineEventId = Guid.NewGuid().ToString();
            CultureInfo ukCulture = new CultureInfo("en-GB");
            EventToCreate.EventDateTime.ToString();
            try
            {
                EventToCreate.EventDateTime = DateTime.Parse(EventToCreate.EventDateTime, ukCulture.DateTimeFormat).Ticks.ToString();
            }
            catch {

                EventToCreate.EventDateTime = null;
                EventToCreate.Title = null;
            }


            string success = API.PutRequest(request, EventToCreate);

            return success;

        }

        public PartialViewResult UploadAttachmentView(string Id)
        {

            CreateAttachment model = new CreateAttachment(Id);
            model.TimelineEventId = Id;
            return PartialView("_UploadAttachment", model);
        }

        #endregion









    }
}