using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Wafer
{
    public class HostRunner
    {
        private readonly HttpServer _server;
        private readonly HttpClient _client;

        public string BaseUrl { get; }

        public HostRunner(Uri baseUri, Action<HttpConfiguration> webApiConfigRegistration)
            : this(baseUri, webApiConfigRegistration, null)
        {
        }

        public HostRunner(
            Uri baseUri, 
            Action<HttpConfiguration> webApiConfigRegistration, 
            Action<HttpConfiguration> dependencyResolverConfigRegistration)
        {
            BaseUrl = baseUri.ToString();
            var config = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
            webApiConfigRegistration(config);
            dependencyResolverConfigRegistration?.Invoke(config);
            _server = new HttpServer(config);
            _client = new HttpClient(_server);
        }

        ~HostRunner()
        {
            _client.Dispose();
            _server.Dispose();
        }

        public async Task<HttpResponseMessage> GetAsync(string virtualUrl)
        {
            return await Retrieve(HttpMethod.Get, virtualUrl);
        }

        public async Task<HttpResponseMessage> HeadAsync(string virtualUrl)
        {
            return await Retrieve(HttpMethod.Head, virtualUrl);
        }

        private async Task<HttpResponseMessage> Retrieve(HttpMethod method, string virtualUrl)
        {
            var request = CreateRequest(virtualUrl, "application/json", method);
            return await _client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string virtualUrl, T data) where T : class
        {
            return await Save(HttpMethod.Post, virtualUrl, data);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string virtualUrl, T data) where T : class
        {
            return await Save(HttpMethod.Put, virtualUrl, data);
        }

        public async Task<HttpResponseMessage> PutAsync(string virtualUrl)
        {
            return await Save<object>(HttpMethod.Put, virtualUrl, null);
        }

        private async Task<HttpResponseMessage> Save<T>(HttpMethod method, string url, T data) where T : class
        {
            var request = CreateRequest(url, "application/json", method, data, new JsonMediaTypeFormatter());
            return await _client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string virtualUrl)
        {
            var request = CreateRequest(virtualUrl, null, HttpMethod.Delete);
            return await _client.SendAsync(request);
        }

        private HttpRequestMessage CreateRequest<T>(
            string virtualUrl, 
            string mediaType, 
            HttpMethod method, 
            T content, 
            MediaTypeFormatter formatter) where T : class
        {
            HttpRequestMessage request = CreateRequest(virtualUrl, mediaType, method);
            request.Content = new ObjectContent<T>(content, formatter);
            return request;
        }

        private HttpRequestMessage CreateRequest(string virtualUrl, string mediaType, HttpMethod method)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(BaseUrl + virtualUrl),
                Method = method
            };
            if (!string.IsNullOrEmpty(mediaType))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            }
            return request;
        }
    }
}
