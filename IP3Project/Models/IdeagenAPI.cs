using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IP3Project.Models
{
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




        //UTILITY REGION OF PRIVATE METHODS
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

        /// <summary>
        /// Sets the AuthToken and TenantId for use in the body of a PUT
        /// </summary>
        /// <param name="x">PutViewModel to have properties set</param>
        /// <returns>PutViewModel with properties set</returns>
        private PutViewModel SetupPutRequestAuthorizations(PutViewModel x)
        {
            x.TenantId = TenantId;
            x.AuthToken = AuthToken;

            return x;
        }

        /// <summary>
        /// Sets the AuthToken and TenantId to headers in a GET 
        /// </summary>
        /// <param name="request">RestRequest object to have headers set</param>
        /// <returns>RestRequest with authorization headers set</returns>
        private RestRequest SetupGetRequestAuthorizations(RestRequest request)
        {
            request.AddHeader("AuthToken", AuthToken);
            request.AddHeader("TenantId", TenantId);

            return request;
        }


        #endregion




    }
}
