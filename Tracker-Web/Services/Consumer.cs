using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Tracker_Web.Services
{
    public enum MethodType
    {
        GET, 
        PUT, 
        POST, 
        DELETE
    }

    public class Consumer
    {
        private static string apiBaseUrl = "https://localhost://1234/";

        public static async Task<T> makeEmptyRequest<T>(HttpClient client, MethodType method, string path)
        {
            var requestMsg = new HttpRequestMessage()
            {
                Method = new HttpMethod(method.ToString()),
                RequestUri = new Uri(apiBaseUrl + path)
            };

            return await sendRequest<T>(requestMsg, client);
        }

        public static async Task<T> makeRequest<T, U>(HttpClient client, MethodType method, string path, U body)
        {
            var requestMsg = new HttpRequestMessage()
            {
                Method = new HttpMethod(method.ToString()),
                Content = JsonContent.Create(body),
                RequestUri = new Uri(apiBaseUrl + path)
            };

            return await sendRequest<T>(requestMsg, client);
        }

        private static async Task<T> sendRequest<T>(HttpRequestMessage requestMsg, HttpClient client)
        {
            var httpResponse = await client.SendAsync(requestMsg);
            string rawResponse = await httpResponse.Content.ReadAsStringAsync();

            T responseBody = JsonConvert.DeserializeObject<T>(rawResponse);
            return responseBody;
        }
    }
}
