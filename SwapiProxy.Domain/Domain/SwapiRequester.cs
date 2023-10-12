using Polly;
using Polly.RateLimit;
using System.Text.Json;

namespace SwapiProxy.Domain
{
    public class SwapiRequester : ISwapiRequester
    {
        private readonly IRateLimiter _rateLimiter;
        private readonly IHttpClientFactory _httpClientFactory;

        public SwapiRequester(IRateLimiter rateLimiter, IHttpClientFactory httpClientFactory)
        {
            _rateLimiter = rateLimiter;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<object> GetAsync(string relativeUrl)
        {
            //TODO: throw proper error if SWAPI client doesn't exist
            //Get the swapi string from a config value
            var httpClient = _httpClientFactory.CreateClient("Swapi");
            var rateLimiter = _rateLimiter.GetPolicy();

            var httpResponseMessage = await rateLimiter.ExecuteAsync(() =>
            {
                Console.WriteLine($"Came into execution {DateTime.Now.ToLongTimeString()}");

                return httpClient.GetAsync(relativeUrl);
            });

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<object>(contentStream);
            }

            return string.Empty;
        }

        public async Task LimitedGet(Action action)
        {
            var retryPolicy = Policy
                .Handle<RateLimitRejectedException>()
                .WaitAndRetry(
                    3,
                    (int _, Exception ex, Context __) => ((RateLimitRejectedException)ex).RetryAfter,
                    (_, __, ___, ____) => { /*Log that the r*/ });

            retryPolicy.Execute(action);
        }
    }
}