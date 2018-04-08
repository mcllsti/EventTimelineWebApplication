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


        public ActionResult DownloadAttachment(string fileName)
        {
            WebClient wc = new WebClient();         

            wc.DownloadFileAsync(new Uri(GenerateGetPreSignedUrl(fileName)), fileName);

            return null;
        }

        private string GenerateGetPreSignedUrl(string fileName)
        {

            var request = new RestRequest("TimelineEventAttachment/GenerateGetPresignedUrl"); //setting up the request params
            request.AddHeader("AttachmentId", fileName);

            var response =(RestResponse) API.GetRequest(request);
            var stream = response.Content;
            var reader = new StreamReader(stream);

            return reader.ReadToEnd();

        }


    }
}