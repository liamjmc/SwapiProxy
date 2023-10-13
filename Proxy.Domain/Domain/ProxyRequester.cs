using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Proxy.Domain
{
    public class ProxyRequester : IProxyRequester
    {
        private readonly IRateLimiter _rateLimiter;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;

        public ProxyRequester(IRateLimiter rateLimiter, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            _rateLimiter = rateLimiter;
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<object?> GetAsync(string relativeUrl)
        {
            //TODO: throw proper error if SWAPI client doesn't exist
            //Get the swapi string from a config value
            var httpClient = _httpClientFactory.CreateClient(_appSettings.ClientName);
            var rateLimiter = _rateLimiter.GetPolicy();

            var httpResponseMessage = await rateLimiter.ExecuteAsync(() =>
            {
                //TODO: tidy this write line up
                Console.WriteLine($"Came into execution {DateTime.Now.ToLongTimeString()}");

                return httpClient.GetAsync(relativeUrl);
            });

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<object>(contentStream);
            }

            //TODO: what to return on an error
            return null;
        }
    }
}