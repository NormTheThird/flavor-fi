using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FlavorFi.Services.FacebookServices
{
    public interface IFacebookClient
    {
        Task<HttpResponseMessage> GetAsync(string accessToken, string endpoint, string args = null);
        Task<object> PostAsync(string accessToken, string endpoint, object data, string args = null);
    }

    public class FacebookClient : IFacebookClient
    {
        private readonly HttpClient _httpClient;

        public FacebookClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v3.2/") };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> GetAsync(string accessToken, string endpoint, string args = null)
        {
            return await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
        }

        public async Task<object> PostAsync(string accessToken, string endpoint, object data, string args = null)
        {
            var payload = GetPayload(data);
            return await _httpClient.PostAsync($"{endpoint}?access_token={accessToken}&{args}", payload);
        }

        private static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}