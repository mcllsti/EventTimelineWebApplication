using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IP3Project.Classes;
using IP3Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace IP3Project.Controllers
{
    public class TimelineController : BaseController
    {
        // GET: Timeline
        public ActionResult Timelines()
        {  
            return View();
        }

        /// <summary>
        /// Makes a request to the API and returns a populated view model with all timelines 
        /// </summary>
        /// <param name="model">TimelineList viewmodel to be populated with a list of timelines</param>
        /// <returns>Populated TimelineList viewmodel</returns>
        private TimelineList GetAllTimelines(TimelineList model)
        {

            var request = new RestRequest("Timeline/GetTimelines"); //setting up the request params
            IRestResponse response = API.GetRequest(request); //Uses IdeagenAPI wrapperclass to make a request and retreives the response

            var resultsDTO = JsonConvert.DeserializeObject<List<Timeline>>(response.Content); //Deserializes the results from the response

            //Populates the viewmodel with relevent results
            foreach (Timeline x in resultsDTO)
            {
                model.timelines.Add(new TimelineViewModel(x.TimelineTitle, x.CreationTimestamp, x.Id));
            }

            return model;
        }



        // GET: Timeline/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Timeline/Create
        public ActionResult CreateTimeline()
        {
            return View();
        }

        // POST: Timeline/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Timelines));
            }
            catch
            {
                return View();
            }
        }

        // GET: Timeline/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Timeline/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Timelines));
            }
            catch
            {
                return View();
            }
        }

        // GET: Timeline/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Timeline/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Timelines));
            }
            catch
            {
                return View();
            }
        }
    }
}