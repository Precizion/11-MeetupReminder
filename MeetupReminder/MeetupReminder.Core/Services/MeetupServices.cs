using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Meetup.Connect;
using Newtonsoft.Json.Linq;
using Spring.Social.OAuth1;
using System.Diagnostics;
using MeetupReminder.Core.Domain;
using Newtonsoft.Json;

namespace MeetupReminder.Core.Services
{
    public class MeetupServices
    {
        private const string MeetupApiKey = "ebgjoqo3ijvijrmiisl7021nck";
        private const string MeetupSecretKey = "8b9424jmeu935r9kp7cp9a52os";

        private static async  Task<OAuthToken> authenticate()
        {
            //setup meetupServiceProvider - this is part of the authentication process (???)
            var meetupServiceProvider = new MeetupServiceProvider(MeetupApiKey, MeetupSecretKey);

            /* OAuth Dance */
            var oauthToken = meetupServiceProvider.OAuthOperations.FetchRequestTokenAsync("oob", null).Result;

            var authenticateUrl = meetupServiceProvider.OAuthOperations.BuildAuthenticateUrl(oauthToken.Value, null);

            Process.Start(authenticateUrl);  //opens up web browser

            Console.WriteLine("Enter the pin from meetup.com");
            string pin = Console.ReadLine();

            var requestToken = new AuthorizedRequestToken(oauthToken, pin);
            var oauthAccessToken = meetupServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;

            return oauthAccessToken;
        }

        //want to return task for async methods
        public static async Task<List<MeetupResults>> GetMeetupsFor(string meetupGroupName)
        {
            // creates token using method authenticate() from above
            var token = await authenticate();

            // creates new MeetupServiceProvider
            var meetupServiceProvider = new MeetupServiceProvider(MeetupApiKey, MeetupSecretKey);

            //what is this line doing? -- puts token into MeetupServiceProvider for authorization
            var meetup = meetupServiceProvider.GetApi(token.Value, token.Secret);

            //await makes the program kind of pause until you get the result of the following code before running everything else
            //creates json string
            string json = await meetup.RestOperations.GetForObjectAsync<string>($"https://api.meetup.com/2/events?&sign=true&photo-host=public&group_urlname={meetupGroupName}&page=20");
            //parses json and assigned to variable o
            var o = JObject.Parse(json);
            //assigns "results" layers of JSON to string resultsJson
            string resultsJson = o["results"].ToString();

            //creates List of MeetupResults objects called result and deserializes the resultsJson string and sticks it into the list
            List<MeetupResults> result = JsonConvert.DeserializeObject<List<MeetupResults>>(resultsJson);

            //returns the List and satisfies the Task condition
            return result;
        }
    }
}
