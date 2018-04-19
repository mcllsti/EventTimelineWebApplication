using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IP3Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NToastNotify;
using RestSharp;
using X.PagedList;

namespace IP3Project.Controllers
{
    /// <summary>
    ///Author: Team16
    ///Date: Trimester 2, 2018 
    ///Version: 1.0 
    /// 
    /// Controller that deals with the interaction,creation,update and deletion of Timelines
    /// 
    /// Extensions used:
    /// Restsharp - API Interaction - https://www.restsharp.org - Apache License 2.0
    /// X.PagedList - Paged Lists in register - https://github.com/dncuug/X.PagedList - MIT License
    /// Newtonsoft.Json - serilization and deserilization of JSON - https://www.newtonsoft.com/json - MIT License
    /// </summary>
    public class TimelineController : BaseController
    {
        private IToastNotification _toastNotification;


        //Dependency  injection and Consructor
        public TimelineController(IOptions<AppSettings> appSettings, IToastNotification toastNotification) : base(appSettings)
        {
            _toastNotification = toastNotification;
        }



        /// <summary>
        /// Contains the logic for prosessing and returning the Timeline Register and its sorting.
        /// </summary>
        /// <param name="sortOrder">String of perticular order of sort</param>
        /// <param name="searchString">String for perticular search from user</param>
        /// <param name="page">Int containg the paged number</param>
        /// <returns>View</returns>
        public IActionResult Timelines(string sortOrder, string searchString, int? page)
        {
            //Setting Viewdata
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            //Attempting to get the timelines to be displayed
            TimelineListViewModel model = new TimelineListViewModel();
            try
            {
                model = GetAllTimelines(model);
            }
            catch
            {
                return RedirectToAction("APIError");
            }


            //Sets to var and determins if there is a search to take place
            //If there is a search then limit the results to the searched paramiters
            var results = from s in model.Timelines
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                results = results.Where(s => s.Title.ToLower().Contains(searchString.ToLower())
                                       || (s.GetDateTime().ToString()).Contains(searchString));
            }

            //Switch for sorting the data in perticular order by user determined
            switch (sortOrder)
            {
                case "name_desc":
                    results = results.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    results = results.OrderBy(s => s.CreationTimeStamp);
                    break;
                case "name":
                    results = results.OrderBy(s => s.Title);
                    break;
                default:
                    results = results.OrderByDescending(s => s.CreationTimeStamp);
                    break;
            }


            //Variable set up for return to view
            var pagenumber = page ?? 1;
            var onepage = results.ToPagedList(pagenumber, 10);
            ViewBag.OnePageOfProducts = onepage;

            return View();
        }

        /// <summary>
        /// Returns partial view for user to Create a Timeline
        /// </summary>
        /// <returns>Partial View</returns>
        [HttpGet]
        public IActionResult CreateTimeline()
        {
            ViewData["Title"] = "Create a Timeline";
            ViewData["Action"] = "CreateTimeline";
            CreateEditViewModel model = new CreateEditViewModel();
            return PartialView("_CreateEditTimeline",model);

        }

        /// <summary>
        /// Recieves model from view and passes to be created through API
        /// </summary>
        /// <param name="model">CreateEditViewModel of Timeline to be created</param>
        /// <returns>Redirect to Timelines view</returns>
        [HttpPost]
        public IActionResult CreateTimeline(CreateEditViewModel model)
        {
            string success = Create(model);
            if(!success.Equals("OK")) //Error handling for robustness
            {
                return RedirectToAction("APIError");
            }

            _toastNotification.AddSuccessToastMessage("Timeline has been created!");
            return RedirectToAction("Timelines"); //returns to the Index!

        }

        /// <summary>
        /// Returns partial view for user to Edit a Timeline
        /// </summary>
        /// <param name="Id">String Id of Timeline to be edited</param>
        /// <param name="Title">String Title of current Timeline Title</param>
        /// <returns>Partial view</returns>
        [HttpGet]
        public PartialViewResult Edit(string Id, string Title)
        {
            //Set Viewdata
            ViewData["Title"] = "Edit Timeline";
            ViewData["Action"] = "Edit";

            CreateEditViewModel model = new CreateEditViewModel(Id,Title); //Create model from recieved variables

            try //Try get specified timeline or return error
            {
                model = GetTimeline(model);
            }
            catch{
                ViewData["message"] = "This error was generated on communicating with the  Please contact your system support.";
                _toastNotification.AddErrorToastMessage("API Error!");
                return PartialView("_Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            return PartialView("_CreateEditTimeline", model);

        }

        /// <summary>
        /// Recieved model from view and passes to be edited through API
        /// </summary>
        /// <param name="model">CreateEditViewModel to be edited</param>
        /// <returns>Redirect to Timelines</returns>
        [HttpPost]
        public IActionResult Edit(CreateEditViewModel model)
        {
            string success = EditTimeline(model); //Edit function to interact with API

            if (!success.Equals("OK")) //Error handling
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddInfoToastMessage("Timeline has edited!");
            return RedirectToAction("Timelines"); //returns to the Index!

        }

        /// <summary>
        /// Deletes a Timeline from the API and returns to the list of Timelines
        /// </summary>
        /// <param name="Id">String Id of timeline to be deleted</param>
        /// <returns>Redirects to Index Action</returns>
        public IActionResult Delete(string Id)
        {

            string IsSuccessful = DeleteTimeline(Id); //Calls delete timeline method which will give a success or failure string
            if (!IsSuccessful.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            _toastNotification.AddWarningToastMessage("Timeline has deleted!");
            return RedirectToAction("Timelines"); //returns to the Index!

        }


        
        #region Utility

        /// <summary>
        /// Makes a request to the API and returns a populated view model with all Timelines 
        /// </summary>
        /// <param name="model">TimelineListViewModel viewmodel to be populated with a list of Timelines</param>
        /// <returns>Populated TimelineListViewModel viewmodel</returns>
        private TimelineListViewModel GetAllTimelines(TimelineListViewModel model)
        {

            var request = new RestRequest("Timeline/GetTimelines"); //setting up the request params
            IRestResponse response = GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

                var resultsDTO = JsonConvert.DeserializeObject<List<Timeline>>(response.Content); //Deserializes the results from the response
                //Populates the viewmodel with relevent results
                foreach (Timeline x in resultsDTO)
                {
                    model.Timelines.Add(x);
                }
            
            return model;
        }




        /// <summary>
        /// Gets a Timeline from the API and populated view model with data
        /// </summary>
        /// <param name="model">CreateEditViewModel to be filled with API Data</param>
        /// <returns>CreateEditViewModel with data of Timeline from API</returns>
        private CreateEditViewModel GetTimeline(CreateEditViewModel model)
        {

            var request = new RestRequest("Timeline/GetTimeline"); //setting up the request params
            request.AddHeader("TimelineId",model.TimelineId);

            IRestResponse response = GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response
                var resultsDTO = JsonConvert.DeserializeObject<Timeline>(response.Content); //Deserializes the results from the response
                model.TimelineId = resultsDTO.Id;
                model.Title = resultsDTO.Title;


            return model;
        }

        /// <summary>
        /// Deletes a timeline by making a request to the api and returns a sucess string
        /// </summary>
        /// <param name="Id">String Id of object to be deleted</param>
        /// <returns>Sucess string</returns>
        private string DeleteTimeline(string Id)
        {

            var request = new RestRequest("Timeline/Delete"); //setting up the request params

            DeleteViewModel DeleteModel = new DeleteViewModel(); //Creates a delete view model to help handle request

            DeleteModel.TimelineId = Id; //Sets the ID of the timeline to be deleted

            var success = PutRequest(request, DeleteModel); //Calls API to do request and gets response content from it

            return success.ToString();

        }

        /// <summary>
        /// Creates a timeline.
        /// </summary>
        /// <returns>The timeline.</returns>
        /// <param name="Title">String Title of the timeline.</param>
        /// <param name="Id">String Id to given to the Timleine.</param>
        private String Create(CreateEditViewModel model)
        {

            var request = new RestRequest("Timeline/Create");

            model.TimelineId = Guid.NewGuid().ToString();

            var success = PutRequest(request, model);

            return success.ToString();


        }

        /// <summary>
        /// Creates a timeline.
        /// </summary>
        /// <returns>The timeline.</returns>
        /// <param name="Title">String Title of the timeline.</param>
        /// <param name="Id">String Id to given to the Timleine.</param>
        private String EditTimeline(CreateEditViewModel model)
        {

            var request = new RestRequest("Timeline/EditTitle");

            var success = PutRequest(request, model);

            return success.ToString();


        }

        #endregion



    }
}