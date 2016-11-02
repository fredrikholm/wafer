using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Wafer
{
    public class HostRunner
    {
        private readonly HttpServer _server;
        private readonly HttpClient _client;
        private readonly Uri _baseUri;

        public HostRunner(Uri baseUri, Action<HttpConfiguration> webApiConfigRegistration)
            : this(baseUri, webApiConfigRegistration, null)
        {
        }

        public HostRunner(Uri baseUri, Action<HttpConfiguration> webApiConfigRegistration, Action<HttpConfiguration> dependencyResolverConfigRegistration)
        {
            _baseUri = baseUri;
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

        public HttpResponseMessage Get(string virtualUrl)
        {
            var request = CreateRequest(virtualUrl, "application/json", HttpMethod.Get);
            return _client.SendAsync(request).Result;
        }

        private HttpRequestMessage CreateRequest(string url, string mediaType, HttpMethod method)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_baseUri + url),
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
