using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IP3Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace IP3Project.Controllers
{
    public class AttachmentController : BaseController
    {

        public async Task<ActionResult> UploadAttachment(CreateAttachment model)
        {

            var title = model.File.FileName;
            var url = GenerateUploadPreSignedUrl(title);

            var httpRequest = WebRequest.Create(url) as HttpWebRequest;
            httpRequest.Method = "PUT";

            var filePath = Path.GetTempFileName();


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

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

            SendCreateAttachmentRequest(title, model.TimelineEventId);
            var response = httpRequest.GetResponse() as HttpWebResponse;
            string hello = "hi";


            return View();
        }

        private void SendCreateAttachmentRequest(string fileName, string EventId)
        {
            var request = new RestRequest("TimelineEventAttachment/Create");

            Attachment AttachmentToCreate = new Attachment(Guid.NewGuid().ToString(), fileName, EventId);

            string success = API.PutRequest(request, AttachmentToCreate);

        }

        private string GenerateUploadPreSignedUrl(string filePath)
        {
            var request = new RestRequest("TimelineEventAttachment/GenerateUploadPresignedUrl");
            var fileName = Path.GetFileName(filePath);

            request.AddHeader("AttachmentId", fileName);

            var success = API.GetRequest(request);

            return success.Content;
        }

        public ActionResult DownloadAttachment(string fileName)
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GenerateGetPreSignedUrl(fileName));
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            return File(resp.GetResponseStream(), resp.ContentType, fileName);
        }

        private Uri GenerateGetPreSignedUrl(string fileName)
        {

            var request = new RestRequest("TimelineEventAttachment/GenerateGetPresignedUrl"); //setting up the request params
            request.AddHeader("AttachmentId", fileName);

            var response = (RestResponse)API.GetRequest(request);
            Uri link = new Uri(response.Content);


            return link;

        }


    }
}