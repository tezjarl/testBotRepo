using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;

namespace Bot_Application3
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            /*if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;*/
            if (activity.Type == ActivityTypes.Message)

            {

                if (activity.Text == "login")

                {

                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));



                    Activity replyToConversation = activity.CreateReply();

                    replyToConversation.Recipient = activity.From;

                    replyToConversation.Type = "message";



                    replyToConversation.Attachments = new List<Attachment>();

                    List<CardAction> cardButtons = new List<CardAction>();
                    GoogleAuthorizationCodeRequestUrl url = new GoogleAuthorizationCodeRequestUrl(new Uri("https://accounts.google.com/o/oauth2/v2/auth"));
                    url.ClientId = "208591536551-cs18qgtasrdjha3vb7b22j09phblif5v.apps.googleusercontent.com";
                    url.AccessType = "offline";
                    url.Scope = CalendarService.Scope.CalendarReadonly;
                    url.RedirectUri = "http://localhost:3979/api/authorize";
                    CardAction plButton = new CardAction()

                    {

                        Value = url.Build(),

                        Type = "signin",

                        Title = "Authentication Required",
                        

                    };

                    cardButtons.Add(plButton);

                    SigninCard plCard = new SigninCard("Please login to Office 365", new List<CardAction>() { plButton });

                    Attachment plAttachment = plCard.ToAttachment();

                    replyToConversation.Attachments.Add(plAttachment);



                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);

                }

                else if (activity.Text == "get mail")

                {

                    // Get access token from bot state

                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    StateClient stateClient = activity.GetStateClient();

                    BotState botState = new BotState(stateClient);

                    BotData botData = await botState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

                    string token = botData.GetProperty<string>("AccessToken");



                    // Get recent 10 e-mail from Office 365

                   /* HttpClient cl = new HttpClient();

                    var acceptHeader =

                        new MediaTypeWithQualityHeaderValue("application/json");

                    cl.DefaultRequestHeaders.Accept.Add(acceptHeader);

                    cl.DefaultRequestHeaders.Authorization

                      = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage httpRes =

                      await cl.GetAsync("https://outlook.office365.com/api/v1.0/me/messages?$orderby=DateTimeSent%20desc&$top=10&$select=Subject,From");

                    if (httpRes.IsSuccessStatusCode)

                    {

                        var strRes = httpRes.Content.ReadAsStringAsync().Result;

                        JObject jRes = await httpRes.Content.ReadAsAsync<JObject>();

                        JArray jValue = (JArray)jRes["value"];

                        foreach (JObject jItem in jValue)

                        {

                            Activity reply = activity.CreateReply($"Subject={((JValue)jItem["Subject"]).Value}");

                            await connector.Conversations.ReplyToActivityAsync(reply);

                        }

                    }

                    else

                    {

                        Activity reply = activity.CreateReply("Failed to get e-mail.\n\nPlease type \"login\" before you get e-mail.");

                        await connector.Conversations.ReplyToActivityAsync(reply);

                    }*/

                }

                else if (activity.Text == "test")
                {
                    UserCredential credential;
                    using (var stream = new FileStream(HttpContext.Current.Server.MapPath("~/client_secret.json"), FileMode.Open, FileAccess.Read))
                    {
                       
                       credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                          GoogleClientSecrets.Load(stream).Secrets,
                          new[] { CalendarService.Scope.CalendarReadonly },
                          "okke000@gmail.com", CancellationToken.None,null, new LocalServerCodeReceiver());
                    }
                    
                   
                }
                else
                {

                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                    Activity reply = activity.CreateReply("# Bot Help\n\nlogin -- Login to Office 365\n\nget mail -- Get your e-mail from Office 365");

                    await connector.Conversations.ReplyToActivityAsync(reply);

                }

            }

            else

            {

                HandleSystemMessage(activity);

            }



            var response = Request.CreateResponse(HttpStatusCode.OK);

            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}