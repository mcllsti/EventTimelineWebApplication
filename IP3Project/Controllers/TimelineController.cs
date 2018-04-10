﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IP3Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using X.PagedList;

namespace IP3Project.Controllers
{
    public class TimelineController : BaseController
    {

        public IActionResult Timelines(string sortOrder, string searchString, int? page)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            TimelineList model = new TimelineList();
            model = GetAllTimelines(model);

            var results = from s in model.Timelines
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                results = results.Where(s => s.Title.Contains(searchString)
                                       || s.CreationTimeStamp.ToString().Contains(searchString));
            }
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



            var pagenumber = page ?? 1;
            var onepage = results.ToPagedList(pagenumber, 5);
            ViewBag.OnePageOfProducts = onepage;
            return View();
        }


        [HttpGet]
        public IActionResult CreateTimeline()
        {
            ViewData["Title"] = "Create a Timeline";
            ViewData["Action"] = "CreateTimeline";
            CreateEditViewModel model = new CreateEditViewModel();
            return PartialView("_CreateEditTimeline",model);

        }

        [HttpPost]
        public IActionResult CreateTimeline(CreateEditViewModel model)
        {
            string success = Create(model);
            if(!success.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
            return RedirectToAction("Timelines"); //returns to the Index!

        }


        [HttpGet]
        public PartialViewResult Edit(string Id, string Title)
        {
            ViewData["Title"] = "Edit Timeline";
            ViewData["Action"] = "Edit";
            CreateEditViewModel model = new CreateEditViewModel(Id,Title);
            model = GetTimeline(model);
            if(model == null)
            {
                ViewData["message"] = "This error was generated on communicating with the API. Please contact your system support.";
                return PartialView("_Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            return PartialView("_CreateEditTimeline", model);

        }

        [HttpPost]
        public IActionResult Edit(CreateEditViewModel model)
        {
            string success = EditTimeline(model);
            if (!success.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }
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
            return RedirectToAction("Timelines"); //returns to the Index!

        }

        #region Utility

        /// <summary>
        /// Makes a request to the API and returns a populated view model with all Timelines 
        /// </summary>
        /// <param name="model">TimelineList viewmodel to be populated with a list of Timelines</param>
        /// <returns>Populated TimelineList viewmodel</returns>
        private TimelineList GetAllTimelines(TimelineList model)
        {

            var request = new RestRequest("Timeline/GetTimelines"); //setting up the request params
            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response



            try
            {
                var resultsDTO = JsonConvert.DeserializeObject<List<Timeline>>(response.Content); //Deserializes the results from the response
                //Populates the viewmodel with relevent results
                foreach (Timeline x in resultsDTO)
                {
                    model.Timelines.Add(x);
                }
            }
            catch
            {
                Response.Redirect("APIError");
            }


            return model;
        }





        private CreateEditViewModel GetTimeline(CreateEditViewModel model)
        {

            var request = new RestRequest("Timeline/GetTimeline"); //setting up the request params
            request.AddHeader("TimelineId",model.TimelineId);

            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            try
            {
                var resultsDTO = JsonConvert.DeserializeObject<Timeline>(response.Content); //Deserializes the results from the response
                model.TimelineId = resultsDTO.Id;
                model.Title = resultsDTO.Title;
            }
            catch
            {
                model = null;
            }

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

            var success = API.PutRequest(request, DeleteModel); //Calls API to do request and gets response content from it

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

            var success = API.PutRequest(request, model);

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

            var success = API.PutRequest(request, model);

            return success.ToString();


        }

        #endregion



    }
}