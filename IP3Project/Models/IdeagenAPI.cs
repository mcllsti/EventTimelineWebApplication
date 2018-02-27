using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IP3Project.Models
{
    /// <summary>
    /// Wrapper Class that bridges the IdeagenAPI and RestSharp togither for ease of use and scaleability
    /// Main purpose for encapsulation
    /// </summary>
    public class IdeagenAPI
    {

        private const string BaseUrl = "https://gcu.ideagen-development.com";
        private const string AuthToken = "3dbd6fb2-caa0-4083-b286-6544baf2a248";
        private const string TenantId = "Team16";
    


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
            request.AddHeader("AuthToken", AuthToken);
            request.AddHeader("TenantId", TenantId);


            IRestResponse response = client.Execute(request); //execures request 

            return response;

        }




        public IRestResponse PutRequest(RestRequest request, Object x)
        {
            //sets up the client and the base URL
            var client = new RestClient();
            client = SetupClientURL(client);

            TrySetProperty(x, "AuthToken", AuthToken);
            TrySetProperty(x, "TenantId", TenantId);

            //Set appropriate request properties

            request.Method = Method.PUT;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(x);


            IRestResponse response = client.Execute(request); //execures request 

            return response;

        }


        #region Utility

        /// <summary>
        /// Sets up a basic client object with the base url
        /// </summary>
        /// <param name="client">RestClient Object to setup with BaseURL</param>
        /// <returns>RestClient object with url set </returns>
        private RestClient SetupClientURL(RestClient client)
        {
            client.BaseUrl = new System.Uri(BaseUrl);
            return client;
        }

        private void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }

        #endregion
    }
}
