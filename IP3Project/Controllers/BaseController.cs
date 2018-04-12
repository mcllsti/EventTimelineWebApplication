using IP3Project.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace IP3Project.Controllers
{

    ///<summary>
    ///Author: Team16
    ///Date: Trimester 2, 2018 
    ///Version: 1.0 
    /// 
    /// Abstract base controller that all controllers will inherit from
    /// 
    /// Restsharp - API Interaction - https://www.restsharp.org - Apache License 2.0
    /// Newtonsoft.Json - serilization and deserilization of JSON - https://www.newtonsoft.com/json - MIT License
    /// </summary>
    public abstract class BaseController : Controller
    {
        public IdeagenAPI API = new IdeagenAPI(); //initilize global API wrapper 

        /// <summary>
        /// Error handling that will be returned when API connection errors occur. 
        /// </summary>
        /// <returns>View with information regarding errorS</returns>
        public IActionResult APIError()
        {
            ViewData["message"] = "This error was generated on communicating with the API. Please contact your system support.";
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Gets the TimelineId of the timeline for an Event
        /// </summary>
        /// <param name="Id">String Id of Event</param>
        /// <returns>String Id of the found timeline</returns>
        protected string GetTimelineID(string Id)
        {
            TimelineList resultsDTO = new TimelineList(); //Model set and populate with data
            resultsDTO = GetSystemData();

            Timeline timeline = new Timeline();
            timeline = resultsDTO.Timelines.Find(x => x.TimelineEvents.Exists(y => y.Id == Id)); //search for timeline with eventId contained in

            return timeline.Id; //return id
        }

        /// <summary>
        /// Gets the entire system data from the API
        /// </summary>
        /// <returns>TimelineList of timelines</returns>
        protected TimelineList GetSystemData()
        {
            var request = new RestRequest("Timeline/GetAllTimelinesAndEvent"); //setting up the request params
            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var resultsDTO = JsonConvert.DeserializeObject<TimelineList>(response.Content); //Deserializes the results from the response

            if (!response.StatusDescription.Equals("OK")) { resultsDTO = null; } //Error check purposes

            return resultsDTO;

        }


    }
}
