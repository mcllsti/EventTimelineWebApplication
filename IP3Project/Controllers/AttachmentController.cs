using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using IP3Project.Models;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Uploads an attachment to an event
        /// </summary>
        /// <param name="model">CreateAttachment of the data of the attachment and event</param>
        /// <returns>Redirect to TimelineView</returns>
        public async Task<ActionResult> UploadAttachment(CreateAttachment model)
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

            if(!API.PutRequest(request, AttachmentToCreate).Equals("OK")) //Error Handling
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

            var success = API.GetRequest(request);

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

            var response = (RestResponse)API.GetRequest(request);
            Uri link = new Uri(response.Content); //Creates URI


            return link;

        }






    }
}