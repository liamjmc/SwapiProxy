using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Proxy.Domain
{
    public class ProxyRequester : IProxyRequester
    {
        private readonly IRateLimiter _rateLimiter;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;
        private readonly ILogger<ProxyRequester> _logger;

        public ProxyRequester(IRateLimiter rateLimiter, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<ProxyRequester> logger)
        {
            _rateLimiter = rateLimiter;
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task<object?> GetAsync(string relativeUrl, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
            {
                throw new ArgumentNullException(nameof(relativeUrl));
            }

            if (string.IsNullOrWhiteSpace(_appSettings.ClientName) || string.IsNullOrWhiteSpace(_appSettings.ClientUrl))
            {
                _logger.LogError($"App settings is not configured correctly.");
                
                throw new ArgumentNullException($"Configuration is not correct.");
            }

            var httpClient = _httpClientFactory.CreateClient(_appSettings.ClientName);
            var rateLimiter = _rateLimiter.GetPolicy();

            try
            {
                var httpResponseMessage = await rateLimiter.ExecuteAsync(async () =>  await httpClient.GetAsync(relativeUrl, cancellationToken));

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                    return await JsonSerializer.DeserializeAsync<object>(contentStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when trying to proxy request with relative url {relativeUrl}");
                
                throw;
            }

            return null;
        }
    }
}