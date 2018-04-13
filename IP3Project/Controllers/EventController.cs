using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IP3Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NToastNotify;
using RestSharp;

namespace IP3Project.Controllers
{    /// <summary>
     ///Author: Team16
     ///Date: Trimester 2, 2018 
     ///Version: 1.0 
     /// 
     /// Controller that deals with the interaction,creation,update and deletion of Events
     /// 
     /// Extensions used:
     /// Restsharp - API Interaction - https://www.restsharp.org - Apache License 2.0
     /// Newtonsoft.Json - serilization and deserilization of JSON - https://www.newtonsoft.com/json - MIT License
     /// </summary>
    public class EventController : BaseController
    {

        private IToastNotification _toastNotification;
        //Dependency  injection and Consructor
        public EventController(IOptions<AppSettings> appSettings, IToastNotification toastNotification) : base(appSettings)
        {
            _toastNotification = toastNotification;
        }

        #region Update

        #region Description

        /// <summary>
        /// Gets the Edit Desciption partial view
        /// </summary>
        /// <param name="Id">String Id of the event to be edited</param>
        /// <param name="Description">String description of the current description</param>
        /// <returns>Partial with model</returns>
        [HttpGet]
        public PartialViewResult EditDescription(string Id, string Description)
        {
            EditDescriptionViewModel model = new EditDescriptionViewModel(Id, Description);
            return PartialView("_EditDescription", model);

        }

        /// <summary>
        /// Edits the desciption for an Event
        /// </summary>
        /// <param name="model">New details to be updated</param>
        /// <returns>Redirect to TimelineView</returns>
        [HttpPost]
        public IActionResult EditDescription(EditDescriptionViewModel model)
        {
            
            try //ErrorHandling
            {
                EditDescriptionCall(model); //Calls to Edit 
            }
            catch
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddInfoToastMessage("The event description has been edited!");
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        /// <summary>
        /// Calls to API to Edit Desciription
        /// </summary>
        /// <param name="model"></param>
        private void EditDescriptionCall(EditDescriptionViewModel model)
        {
            var request = new RestRequest("TimelineEvent/EditDescription");

            if(!PutRequest(request, model).Equals("OK")) //Error Handling
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
        }

        #endregion

        #region Date
        /// <summary>
        /// Returns partial form for editing date
        /// </summary>
        /// <param name="Id">String Id of Event with date to edit</param>
        /// <param name="Date">String current Date</param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult EditDate(string Id, string Date)
        {
            EditDateViewModel model = new EditDateViewModel(Id,Date);
            return PartialView("_EditDate", model);

        }

        /// <summary>
        /// Handles the editing of an event date
        /// </summary>
        /// <param name="model">EditDateViewModel with editing details for event</param>
        /// <returns>Redirect to TimelineView</returns>
        [HttpPost]
        public IActionResult EditDate(EditDateViewModel model)
        {
            try //Error handling
            {
                EditDateCall(model); //Calls Edit Date to API
            }
            catch
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddInfoToastMessage("The event date has been edited!");
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        /// <summary>
        /// Calls API to edit date
        /// </summary>
        /// <param name="model">EditDateViewModel model containing the data to update</param>
        private void EditDateCall(EditDateViewModel model)
        {
            model.EventDateTime.ToString();
            CultureInfo ukCulture = new CultureInfo("en-GB"); //Handles international dates

            try //Error handling for date parsing
            {
                model.EventDateTime = DateTime.Parse(model.EventDateTime, ukCulture.DateTimeFormat).Ticks.ToString(); //Parse Date
            }
            catch
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }

            var request = new RestRequest("TimelineEvent/EditEventDateTime");

            if(!PutRequest(request, model).Equals("OK")) //Error Handling
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
        }

        #endregion

        #region Location

        /// <summary>
        /// Returns partial form for editing location
        /// </summary>
        /// <param name="Id">String Id of event to have location edited</param>
        /// <param name="Location">String Location for current event</param>
        /// <returns>Partial with model</returns>
        [HttpGet]
        public PartialViewResult EditLocation(string Id, string Location)
        {
            EditLocationViewModel model = new EditLocationViewModel(Id, Location);
            return PartialView("_EditLocation", model);

        }

        /// <summary>
        /// Calls edit location of the API
        /// </summary>
        /// <param name="model">EditLocationViewModel model for data from form</param>
        /// <returns>Redirect to TimelineView</returns>
        [HttpPost]
        public IActionResult EditLocation(EditLocationViewModel model)
        {
            try //Error handling
            {
                EditLocationCall(model); //Calls Editlocation
            }
            catch
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddInfoToastMessage("The event location has been edited!");
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        /// <summary>
        /// Calls API to edit location
        /// </summary>
        /// <param name="model">EditLocationViewModel model containing data to be updated</param>
        private void EditLocationCall(EditLocationViewModel model)
        {
            var request = new RestRequest("TimelineEvent/EditLocation");

            if(!PutRequest(request, model).Equals("OK")) //Error handling
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
        }

        /// <summary>
        /// Removes a location from an Event
        /// </summary>
        /// <param name="Id">Id of event to have location removed</param>
        /// <returns>Redirect to TimelineView</returns>
        public IActionResult RemoveLocation(string Id)
        {
            EditLocationViewModel model = new EditLocationViewModel(Id, null); //creates model with null to have location removed

            var request = new RestRequest("TimelineEvent/EditLocation");

            if(!(PutRequest(request, model)).Equals("OK")) //error handling
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddWarningToastMessage("The event location has been removed!");
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!
        }



        #endregion

        #region Title

        /// <summary>
        /// Returns partial from for editing a Title
        /// </summary>
        /// <param name="Id">String Id of event to have the Title to be edited</param>
        /// <param name="Title">String title of current event</param>
        /// <returns>Partial with model</returns>
        [HttpGet]
        public PartialViewResult EditTitle(string Id, string Title)
        {
            EditTitleViewModel model = new EditTitleViewModel(Id, Title);
            return PartialView("_EditTitle", model);

        }

        /// <summary>
        /// Recieves form data and calls to Edit Title
        /// </summary>
        /// <param name="model">EditTitleViewModel model with data to update</param>
        /// <returns>Redirect to TimelineView</returns>
        [HttpPost]
        public IActionResult EditTitle(EditTitleViewModel model)
        {
            try //Error handling
            {
                EditTitleCall(model); //call to function for API
            }
            catch
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddInfoToastMessage("The event title has been edited!");
            return RedirectToAction("TimelineView", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!

        }

        /// <summary>
        /// Calls to API to edit Title
        /// </summary>
        /// <param name="model">EditTitleViewModel model to be serilized</param>
        private void EditTitleCall(EditTitleViewModel model)
        {
            var request = new RestRequest("TimelineEvent/EditTitle");

            if(!PutRequest(request, model).Equals("OK")) //Error Handling
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
        }

        #endregion


        #endregion

        #region Create
        /// <summary>
        /// Shows the Create Event partial view form
        /// </summary>
        /// <param name="Id">String Id of the Timeline for the event</param>
        /// <returns>Partial View with model</returns>
        [HttpGet]
        public PartialViewResult CreateEvent(string Id)
        {
            ViewData["Title"] = "Create a Event"; //Sets View data
            ViewData["Action"] = "CreateEvent";

            CreateEventViewModel model = new CreateEventViewModel(Id);

            return PartialView("_CreateEvent", model);

        }

        /// <summary>
        /// Creates and Event with the API
        /// </summary>
        /// <param name="model">CreateEventview with data containg event to be Created</param>
        /// <returns>Redirect to TimelineView</returns>
        [HttpPost]
        public IActionResult CreateEvent(CreateEventViewModel model)
        {
            
            try //Error handling
            {
                //Creates and Links
                Create(model);
                Link(model);
            }
            catch
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddSuccessToastMessage("Event has been created!");
            return RedirectToAction("TimelineView", new { id = model.TimelineId }); //returns to the Index!

        }


        #endregion

        #region Delete
        /// <summary>
        /// Deletes an Event
        /// </summary>
        /// <param name="Id">String Id of event to be deleted</param>
        /// <returns>Redirect to the TimelineView view with String Id</returns>
        public IActionResult DeleteEvent(string Id)
        {

            string TimelineId = GetTimelineID(Id); //Gets the timeline Id
            
            try //Error handling with API
            {
                UnlinkEvent(TimelineId, Id);
                Delete(Id);
            }
            catch
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddWarningToastMessage("Event has been deleted!");
            return RedirectToAction("TimelineView", new { id = TimelineId }); //returns to the TimlineView!
        }
        #endregion

        #region Read

        /// <summary>
        /// Returns the TimelineView for the overall view of a Timeline with events
        /// </summary>
        /// <param name="id">String Id of the Timeline</param>
        /// <returns>View with model</returns>
        [HttpGet]
        public ActionResult TimelineView(string id)
        {
            Timeline model = new Timeline(id); //Set viewModel
            model = GetTimeline(model);

            try //Error Handling
            {
                model.TimelineEvents = GetEvents(id).Events;
            }
            catch
            {
                return RedirectToAction("APIError");
            }

            return View(model);
        }

        /// <summary>
        /// Returns an partial containing Event Details
        /// </summary>
        /// <param name="Id">String Id of event to display</param>
        /// <returns>PartialView with model</returns>
        public IActionResult DisplayEvent(string Id)
        {
            Event EventToDisplay = new Event();
            
            try{ //Ensures no error
                EventToDisplay = GetEvent(Id); //Gets Event
            }
            catch
            {
                return RedirectToAction("APIError");
            }

            return PartialView("_EventView", EventToDisplay);
        }
        #endregion

        /// <summary>
        /// Returns the partial view for Uploading Attachments
        /// </summary>
        /// <param name="Id">Sting Id of Model for attachment to be created</param>
        /// <returns></returns>
        public PartialViewResult UploadAttachmentView(string Id)
        {

            CreateAttachmentViewModel model = new CreateAttachmentViewModel(Id);
            model.TimelineEventId = Id;
            return PartialView("_UploadAttachment", model);
        }

        #region Utility

        /// <summary>
        /// Calls the API to Link Events to an Timeline
        /// </summary>
        /// <param name="EventToLink">CreateEventview of the evnt to be linked</param>
        private void Link(CreateEventViewModel EventToLink)
        {

            TimelineEventLink LinkToCreate = new TimelineEventLink(EventToLink.TimelineId, EventToLink.TimelineEventId); //Creates model for serlization

            var request = new RestRequest("/Timeline/LinkEvent");

            if(!PutRequest(request, LinkToCreate).Equals("OK")) //Error Handling
            {

                throw new System.ArgumentException("Parameter cannot be null", "original");
            }



        }

        /// <summary>
        /// Gets a single Timeline from the API
        /// </summary>
        /// <param name="model">Timeline partially filled with data of timeline to be retreived</param>
        /// <returns>Timeline with specififed details</returns>
        private Timeline GetTimeline(Timeline model)
        {

            var request = new RestRequest("Timeline/GetTimeline"); //setting up the request params
            request.AddHeader("TimelineId", model.Id);

            IRestResponse response = GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response
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

        /// <summary>
        /// Gets events from the system data for the Timeline with specified Id
        /// </summary>
        /// <param name="Id">string Id of timeline for event to be retreived</param>
        /// <returns></returns>
        private EventListViewModel GetEvents(string Id)
        {
            EventListViewModel model = new EventListViewModel(); //set models
            TimelineListViewModel resultsDTO = new TimelineListViewModel();

            resultsDTO = GetSystemData(); //Gets entire system data 

            if(resultsDTO == null) //Error check
            {
                model = null;
                return model;
            }

            List<Event> EventsList = new List<Event>();

            foreach (Timeline x in resultsDTO.Timelines.Where(x => x.Id.Equals(Id)))
            {
                model.Events = x.TimelineEvents; //Inserts events into model
            }

            model.Events = model.Events.OrderBy(o => o.EventDateTime).ToList();

            return model;
        }

        /// <summary>
        /// Deleted an Timeline Event
        /// </summary>
        /// <param name="Id">String Id of event to be deleted</param>
        private void Delete(string Id)
        {
            DeleteEventViewModel delete = new DeleteEventViewModel(Id);
            var request = new RestRequest("TimelineEvent/Delete");

            if (!PutRequest(request, delete).Equals("OK")) //Error Handling
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
     

        }

        /// <summary>
        /// Gets an single event
        /// </summary>
        /// <param name="Id">String Id of the Event to get</param>
        /// <returns>Event model of event retreived</returns>
        private Event GetEvent(string Id)
        {

            var request = new RestRequest("TimelineEvent/GetTimelineEvent"); //setting up the request params
            request.AddHeader("TimelineEventId", Id);

            IRestResponse response = GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var model = JsonConvert.DeserializeObject<Event>(response.Content); //Deserializes the results from the response

            if(!response.StatusDescription.Equals("OK")) //Error Handling
           {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
            List<Attachment> Attachments = new List<Attachment>();
            model.Attachments = GetEventAttachments(Id); //Gets attachments for event

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

            IRestResponse response = GetRequest(request); //API call

            var temp = JsonConvert.DeserializeObject<List<AttachmentViewModel>>(response.Content);
            List<Attachment> model = new List<Attachment>();

            foreach(AttachmentViewModel y in temp)
            {
                model.Add(new Attachment(y.Id, y.Title, y.TimelineEventId));
            }
            return model;
        }

        /// <summary>
        /// Unlinks an event from a timeline
        /// </summary>
        /// <param name="TimelineId">String Id of timeline to unlink event from</param>
        /// <param name="EventId">String Id of event to be unlinked</param>
        private void UnlinkEvent(string TimelineId, string EventId)
        {
            var request = new RestRequest("Timeline/UnlinkEvent");
            TimelineEventLink Unlink = new TimelineEventLink(TimelineId, EventId); //sets model for serlization

            if(!PutRequest(request, Unlink).Equals("OK"))
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }

        }

        /// <summary>
        /// Creates a new event by calling the API
        /// </summary>
        /// <param name="EventToCreate">CreateEventViewModel of event to be created</param>
        private void Create(CreateEventViewModel EventToCreate)
        {
            var request = new RestRequest("TimelineEvent/Create");

            EventToCreate.TimelineEventId = Guid.NewGuid().ToString();

            CultureInfo ukCulture = new CultureInfo("en-GB"); //Handles internation Date errors
            EventToCreate.EventDateTime.ToString();

            try //Error handling. If date cannot be set then ensure model will be blank and API interaction will fail.
            {
                EventToCreate.EventDateTime = DateTime.Parse(EventToCreate.EventDateTime, ukCulture.DateTimeFormat).Ticks.ToString();
            }
            catch {

                EventToCreate.EventDateTime = null;
                EventToCreate.Title = null;
            }

            if(!PutRequest(request, EventToCreate).Equals("OK"))
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");

            }

        }

        #endregion


    }
}