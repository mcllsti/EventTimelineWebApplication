using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IP3Project.Controllers
{
    public class TimelineController : Controller
    {
        // GET: Timeline
        public ActionResult Index()
        {
            return View();
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

                return RedirectToAction(nameof(Index));
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

                return RedirectToAction(nameof(Index));
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

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}