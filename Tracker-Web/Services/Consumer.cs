using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Tracker_Web.Services
{
    public class Consumer : IApiConsumer
    {
        private static string API_BASE_URI = "https://localhost:44391";

        private readonly HttpClient client;
        private readonly IJSRuntime jsInterop;

        public Consumer(HttpClient client, IJSRuntime jsInterop)
        {
            this.client = client;
            this.jsInterop = jsInterop;
        }

        public async Task<(T, HttpStatusCode)> MakeEmptyRequest<T>(MethodType method, string path)
        {
            var requestMsg = new HttpRequestMessage()
            {
                Method = new HttpMethod(method.ToString()),
                RequestUri = new Uri(API_BASE_URI + path)
            };

            return await SendRequest<T>(requestMsg, client);
        }

        public async Task<(T, HttpStatusCode)> MakeRequest<T, U>(MethodType method, string path, U body)
        {
            var requestMsg = new HttpRequestMessage()
            {
                Method = new HttpMethod(method.ToString()),
                Content = JsonContent.Create(body),
                RequestUri = new Uri(API_BASE_URI + path)
            };

            return await SendRequest<T>(requestMsg, client);
        }

        private async Task<(T, HttpStatusCode)> SendRequest<T>(HttpRequestMessage requestMsg, HttpClient client)
        {
            // get session ID and add it to header if it exists
            string sessionIDRaw = await jsInterop.InvokeAsync<string>("readCookie", args: new object[]
                { "sessionID" });
            if (sessionIDRaw == null || sessionIDRaw == "")
            {
                sessionIDRaw = "none";
            }

            requestMsg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                sessionIDRaw);

            var httpResponse = await client.SendAsync(requestMsg);
            string rawResponse = await httpResponse.Content.ReadAsStringAsync();

            T responseBody = JsonConvert.DeserializeObject<T>(rawResponse);
            return (responseBody, httpResponse.StatusCode);
        }
    }
}
