using FlavorFi.Common.Models.FacebookModels;
using FlavorFi.Common.RequestAndResponses.FacebookRequestAndResponses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlavorFi.Services.FacebookServices
{
    public interface IFacebookPageService
    {
        GetSubscribedAppsResponse GetSubscribedApps(GetSubscribedAppsRequest request);
    }

    public class FacebookPageService : IFacebookPageService
    {
        private readonly IFacebookClient _facebookClient;
        private readonly string _pageAccessToken;

        public FacebookPageService(IFacebookClient facebookClient)
        {
            _facebookClient = facebookClient;
            _pageAccessToken = "EAADmagg57yABANyPfY36QGuceqCQJdGd4RuywKwaQHBmceZAHU1RANggnyRa8iHTeMxK8kJDNCJsdcjNubscQZBXMxddUnIGvA1mkBvot45ZAVIJWr0Yu3WVns54UNsml0LoVYlzPpn6ix6CBgjnrwfeyLwmDAnM2OhCiUxBKMZCpQW96GzXdZCSki9PCrJqAWdWZBRjy6wwZDZD";
        }

        public GetSubscribedAppsResponse GetSubscribedApps(GetSubscribedAppsRequest request)
        {
            try
            {
                var retval = _facebookClient.GetAsync(_pageAccessToken, $"{request.PageId}/subscribed_apps", null).Result;
                if (!retval.IsSuccessStatusCode)
                    return new GetSubscribedAppsResponse { ErrorMessage = retval.ReasonPhrase };

                var response = new GetSubscribedAppsResponse { IsSuccess = true };
                var content = retval.Content.ReadAsStringAsync().Result;
                var jsonObject = JObject.Parse(content);
                response.SubscribedApps = JsonConvert.DeserializeObject<List<SubscribedAppModel>>(jsonObject["data"].ToString());
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}