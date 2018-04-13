using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using IP3Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using RestSharp;

namespace IP3Project.Controllers
{    /// <summary>
     ///Author: Team16
     ///Date: Trimester 2, 2018 
     ///Version: 1.0 
     /// 
     /// Controller that deals with the interaction,creation,update and deletion of Timelines
     /// 
     /// Extensions used:
     /// Restsharp - API Interaction - https://www.restsharp.org - Apache License 2.0
     /// </summary>
    public class AttachmentController : BaseController
    {

        private IToastNotification _toastNotification;

        //Dependency  injection and Consructor
        public AttachmentController(IOptions<AppSettings> appSettings, IToastNotification toastNotification) : base(appSettings)
        {
            _toastNotification = toastNotification;
        }

        /// <summary>
        /// Uploads an attachment to an event
        /// </summary>
        /// <param name="model">CreateAttachmentViewModel of the data of the attachment and event</param>
        /// <returns>Redirect to TimelineView</returns>
        public async Task<ActionResult> UploadAttachment(CreateAttachmentViewModel model)
        {

            //Sets Title and URL using fileName and getting presigned URL
            var title = model.File.FileName;
            var url = "";

            try //Error Handling
            {
                url = GenerateUploadPreSignedUrl(title);
            }
            catch
            {

                return RedirectToAction("APIError");
            }
            

            //Sets up webRequest variable
            var httpRequest = WebRequest.Create(url) as HttpWebRequest;
            httpRequest.Method = "PUT";

            var filePath = Path.GetTempFileName();

            //Copies file to tempoary folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            //Writes the file to the Datastream 
            using (var dataStream = httpRequest.GetRequestStream())
            {
                var buffer = new byte[18000];
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dataStream.Write(buffer, 0, bytesRead);
                    }
                }
            }

            //Creates the attachment

            try //Error Handling
            {
                SendCreateAttachmentRequest(title, model.TimelineEventId);
            }
            catch
            {
                return RedirectToAction("APIError");
            }
            
            var response = httpRequest.GetResponse() as HttpWebResponse;
            if(!response.StatusDescription.Equals("OK"))
            {
                return RedirectToAction("APIError");
            }

            _toastNotification.AddSuccessToastMessage("Attachment has uploaded!");
            return RedirectToAction("TimelineView","Event", new { id = GetTimelineID(model.TimelineEventId) }); //returns to the Index!
        }

        /// <summary>
        /// Creates and attachment through API
        /// </summary>
        /// <param name="fileName">String Filename of attachment</param>
        /// <param name="EventId">String Event Id to create attachment on</param>
        private void SendCreateAttachmentRequest(string fileName, string EventId)
        {
            var request = new RestRequest("TimelineEventAttachment/Create");

            Attachment AttachmentToCreate = new Attachment(Guid.NewGuid().ToString(), fileName, EventId);

            if(!PutRequest(request, AttachmentToCreate).Equals("OK")) //Error Handling
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }

        }

        /// <summary>
        /// Calls API to generate uploade presigned URL
        /// </summary>
        /// <param name="filePath">String filePath of file</param>
        /// <returns>Returns Presigned URL in String</returns>
        private string GenerateUploadPreSignedUrl(string filePath)
        {
            var request = new RestRequest("TimelineEventAttachment/GenerateUploadPresignedUrl");
            var fileName = Path.GetFileName(filePath); //Gets path

            request.AddHeader("AttachmentId", fileName);

            var success = GetRequest(request);

            if(!success.StatusDescription.Equals("OK")) //Error Handling
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }

            return success.Content; //Returns URL
        }

        /// <summary>
        /// Downloads an attachment from the URL
        /// </summary>
        /// <param name="fileName">String Filename to Download</param>
        /// <returns>File for the browser to download</returns>
        public ActionResult DownloadAttachment(string fileName)
        {

            //Creates a WebRequest with the presigned URL
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GenerateGetPreSignedUrl(fileName));

            //Creates WebResponse from responce of Web REquest
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            return File(resp.GetResponseStream(), resp.ContentType, fileName);
        }

        /// <summary>
        /// Generates a presigned URL and returns it as a URI
        /// </summary>
        /// <param name="fileName">Stinr fileName of the file to upload</param>
        /// <returns>URI link that was generated</returns>
        private Uri GenerateGetPreSignedUrl(string fileName)
        {

            var request = new RestRequest("TimelineEventAttachment/GenerateGetPresignedUrl"); //setting up the request params
            request.AddHeader("AttachmentId", fileName);

            var response = GetRequest(request);
            Uri link = new Uri(response.Content); //Creates URI


            return link;

        }

        /// <summary>
        /// Deletes an attachment from an event
        /// </summary>
        /// <param name="Id">String id of the attachment</param>
        /// <param name="EventId">String Id of the event containing the attachment</param>
        /// <returns>Redirect to timeline view</returns>
        public IActionResult DeleteAttachment(string Id,string EventId)
        {
            var request = new RestRequest("TimelineEventAttachment/Delete");

            DeleteAttachmentViewModel DeleteMe = new DeleteAttachmentViewModel(Id);
            if (!PutRequest(request, DeleteMe).Equals("OK")) //Error Handling
            {
                return RedirectToAction("ErrorAPI");
            }

            _toastNotification.AddWarningToastMessage("Attachment has been deleted!");
            return RedirectToAction("TimelineView", "Event", new { id = GetTimelineID(EventId) }); //returns to the Index!
        }






    }
}