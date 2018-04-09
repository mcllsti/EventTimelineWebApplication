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
            EditDescriptionCall(model);
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private void EditDescriptionCall(EditDescription model)
        {
            var request = new RestRequest("TimelineEvent/EditDescription");
            API.PutRequest(request, model);
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
            EditDateCall(model);
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private void EditDateCall(EditDate model)
        {
            model.EventDateTime.ToString();
            model.EventDateTime = DateTime.Parse(model.EventDateTime).Ticks.ToString();
            var request = new RestRequest("TimelineEvent/EditEventDateTime");
            API.PutRequest(request, model);
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
            EditLocationCall(model);
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private void EditLocationCall(EditLocation model)
        {
            var request = new RestRequest("TimelineEvent/EditLocation");
            API.PutRequest(request, model);
        }

        public IActionResult RemoveLocation(string Id)
        {
            EditLocation model = new EditLocation(Id, null);
            var request = new RestRequest("TimelineEvent/EditLocation");
            API.PutRequest(request, model);
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!
        }



        #endregion

        #region Title

        #endregion
        [HttpGet]
        public PartialViewResult EditTitle(string Id, string Title)
        {
            EditTitle model = new EditTitle(Id, Title);
            return PartialView("_EditTitle", model);

        }

        [HttpPost]
        public IActionResult EditTitle(EditTitle model)
        {
            EditTitleCall(model);
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        private void EditTitleCall(EditTitle model)
        {
            var request = new RestRequest("TimelineEvent/EditTitle");
            API.PutRequest(request, model);
        }
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
            Create(model);
            Link(model);
            return RedirectToAction("TimelineView", new { id = model.TimelineId }); //returns to the Index!

        }
        #endregion

        #region Delete
        public IActionResult DeleteEvent(string Id)
        {
            string TimelineId = GetTimelineID(Id);
            UnlinkEvent(TimelineId, Id);
            Delete(Id);

            return RedirectToAction("TimelineView", new { id = TimelineId }); //returns to the Index!
        }
        #endregion

        #region Read

        [HttpGet]
        public ActionResult TimelineView(string id)
        {
            Timeline model = new Timeline(id);
            model = GetTimeline(model);
            model.TimelineEvents = GetEvents(id).Events;

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

            return PartialView("_EventView", EventToDisplay);
        }
        #endregion

        #region Utility

        private CreateEventView Link(CreateEventView EventToLink)
        {

            TimelineEventLink LinkToCreate = new TimelineEventLink(EventToLink.TimelineId, EventToLink.TimelineEventId);

            var request = new RestRequest("/Timeline/LinkEvent");
            API.PutRequest(request, LinkToCreate);

            return EventToLink;

        }

        private Timeline GetTimeline(Timeline model)
        {

            var request = new RestRequest("Timeline/GetTimeline"); //setting up the request params
            request.AddHeader("TimelineId", model.Id);

            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            model = JsonConvert.DeserializeObject<Timeline>(response.Content); //Deserializes the results from the response

            return model;
        }

        private EventList GetEvents(string Id)
        {
            TimelineList resultsDTO = new TimelineList();
            resultsDTO = GetSystemData();

            EventList model = new EventList();

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

        private void Delete(string Id)
        {
            DeleteEventViewModel delete = new DeleteEventViewModel(Id);
            var request = new RestRequest("TimelineEvent/Delete");

            API.PutRequest(request, delete);
        }

        private Event GetEvent(string Id)
        {

            var request = new RestRequest("TimelineEvent/GetTimelineEvent"); //setting up the request params
            request.AddHeader("TimelineEventId", Id);

            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var model = JsonConvert.DeserializeObject<Event>(response.Content); //Deserializes the results from the response

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

        private void UnlinkEvent(string TimelineId, string EventId)
        {
            var request = new RestRequest("Timeline/UnlinkEvent");
            TimelineEventLink Unlink = new TimelineEventLink(TimelineId, EventId);

            API.PutRequest(request, Unlink);

        }

        private CreateEventView Create(CreateEventView EventToCreate)
        {
            var request = new RestRequest("TimelineEvent/Create");

            EventToCreate.TimelineEventId = Guid.NewGuid().ToString();
            EventToCreate.EventDateTime.ToString();
            EventToCreate.EventDateTime = DateTime.Parse(EventToCreate.EventDateTime).Ticks.ToString();

            API.PutRequest(request, EventToCreate);

            return EventToCreate;

        }

        public PartialViewResult UploadAttachmentView(string id)
        {

            CreateAttachment model = new CreateAttachment(null, null, id);

            return PartialView("_UploadAttachment", model);
        }

        #endregion









    }
}