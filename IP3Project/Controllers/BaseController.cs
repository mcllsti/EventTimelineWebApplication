using IP3Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    public class BaseController : Controller
    {


        //Dependency  injection and Consructor
        private readonly AppSettings _appSettings;

        public BaseController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        /// <summary>
        /// Error handling that will be returned when API connection errors occur. 
        /// </summary>
        /// <returns>View with information regarding errorS</returns>
        public IActionResult APIError()
        {
            ViewData["message"] = "This error was generated on communicating with the API Please contact your system support.";
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Gets the TimelineId of the timeline for an Event
        /// </summary>
        /// <param name="Id">String Id of Event</param>
        /// <returns>String Id of the found timeline</returns>
        protected string GetTimelineID(string Id)
        {
            TimelineListViewModel resultsDTO = new TimelineListViewModel(); //Model set and populate with data
            resultsDTO = GetSystemData();

            Timeline timeline = new Timeline();
            timeline = resultsDTO.Timelines.Find(x => x.TimelineEvents.Exists(y => y.Id == Id)); //search for timeline with eventId contained in

            return timeline.Id; //return id
        }

        /// <summary>
        /// Gets the entire system data from the API
        /// </summary>
        /// <returns>TimelineListViewModel of timelines</returns>
        protected TimelineListViewModel GetSystemData()
        {
            var request = new RestRequest("Timeline/GetAllTimelinesAndEvent"); //setting up the request params
            IRestResponse response = GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var resultsDTO = JsonConvert.DeserializeObject<TimelineListViewModel>(response.Content); //Deserializes the results from the response

            if (!response.StatusDescription.Equals("OK")) { resultsDTO = null; } //Error check purposes

            return resultsDTO;

        }

        /// <summary>
        /// Executes a Get Request to the API
        /// </summary>
        /// <param name="request">RestRequest Object that has had endpoint set</param>
        /// <returns></returns>
        public IRestResponse GetRequest(RestRequest request)
        {
            //sets up the client and the base URL
            var client = new RestClient();
            client = SetupClientURL(client);

            //Set appropriate request properties
            request.Method = Method.GET;
            request = SetupGetRequestAuthorizations(request);



            IRestResponse response = client.Execute(request); //execures request 

            return response;

        }

        /// <summary>
        /// Executes a Put Request to the API
        /// </summary>
        /// <param name="request">RestRequest Object that has had the endpoint set</param>
        /// <param name="x">Object that has had data set to be passed into the request body</param>
        /// <returns>String content of the response for output</returns>
        public string PutRequest(RestRequest request, PutViewModel x)
        {
            //sets up the client and the base URL
            var client = new RestClient();
            client = SetupClientURL(client);

            //Setting the authtoken and tenantID in private method
            x = SetupPutRequestAuthorizations(x);

            //Set appropriate request properties
            request.Method = Method.PUT;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(x);

            IRestResponse response = client.Execute(request); //execures request 

            return response.StatusDescription;

        }




        /// <summary>
        /// Sets up a basic client object with the base url
        /// </summary>
        /// <param name="client">RestClient Object to setup with BaseURL</param>
        /// <returns>RestClient object with url set </returns>
        protected RestClient SetupClientURL(RestClient client)
        {
            client.BaseUrl = new System.Uri(_appSettings.BaseUrl);
            return client;
        }

        /// <summary>
        /// Sets the AuthToken and TenantId for use in the body of a PUT
        /// </summary>
        /// <param name="x">PutViewModel to have properties set</param>
        /// <returns>PutViewModel with properties set</returns>
        protected PutViewModel SetupPutRequestAuthorizations(PutViewModel x)
        {
            x.TenantId = _appSettings.TenantId;
            x.AuthToken = _appSettings.AuthToken;

            return x;
        }

        /// <summary>
        /// Sets the AuthToken and TenantId to headers in a GET 
        /// </summary>
        /// <param name="request">RestRequest object to have headers set</param>
        /// <returns>RestRequest with authorization headers set</returns>
        protected RestRequest SetupGetRequestAuthorizations(RestRequest request)
        {
            request.AddHeader("AuthToken", _appSettings.AuthToken);
            request.AddHeader("TenantId", _appSettings.TenantId);

            return request;
        }



    }
}
