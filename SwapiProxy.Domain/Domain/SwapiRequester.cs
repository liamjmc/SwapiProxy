using System.Net.Http;
using System.Text.Json;

namespace SwapiProxy.Domain
{
    public class SwapiRequester : ISwapiRequester
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SwapiRequester(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<object> GetAsync(string relativeUrl)
        {
            //TODO: throw proper error if SWAPI client doesn't exist
            var httpClient = _httpClientFactory.CreateClient("Swapi");

            var httpResponseMessage = await httpClient.GetAsync(relativeUrl);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<object>(contentStream);
            }

            return string.Empty;
        }
    }
}   