using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ProviderService.Clients
{
    public class WebApiClient<T>
    {
        private readonly Uri _uri;

        public WebApiClient(Uri uri)
        {
            _uri = uri;
        }

        public T GetData(string resource)
        {
            // init http client
            using (var client = new HttpClient { BaseAddress = new Uri(_uri.AbsoluteUri) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // send response and get request
                var response = client.GetAsync(resource).Result;
                response.EnsureSuccessStatusCode();

                // if OK:
                return response.Content.ReadAsAsync<T>().Result;
            }
        }
    }
}
