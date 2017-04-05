using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Http;
using Microsoft.Bot.Connector;

namespace Bot_Application3.Controllers
{
    public class AuthorizeController : ApiController
    {
        [Route("api/authorize")]
        public Task<HttpResponseMessage> Get([FromUri] string code)
        {
            var ctx = HttpContext.Current;
            AuthorizationCodeTokenRequest request = new AuthorizationCodeTokenRequest(); 
            request.Code = code;
            request.ClientId = "208591536551-cs18qgtasrdjha3vb7b22j09phblif5v.apps.googleusercontent.com";
            request.ClientSecret = "cDfAZUQ8FaGm3QwstTwsut93";
            request.RedirectUri = @"http://localhost:3979/api/messages";
            var res = request.ExecuteAsync(new HttpClient(), @"https://accounts.google.com/o/oauth2/token",
                    new CancellationToken(), null).Result;
            return new Task<HttpResponseMessage>(() =>
            {
                
                return new HttpResponseMessage();
            });

            
        }
        
    }
}
