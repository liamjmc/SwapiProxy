using Polly;
using Proxy.Domain;

namespace SwapiProxy.API.ServiceExtensions
{
    public static class HttpClientServiceExtensions
    {
        public static void AddHttpClient(this WebApplicationBuilder builder)
        {
            var appSettingsConfiguration = builder.Configuration.GetSection("AppSettings");
            var appSettings = appSettingsConfiguration.Get<AppSettings>();

            builder.Services.Configure<AppSettings>(appSettingsConfiguration);

            builder.Services.AddHttpClient(appSettings.ClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(appSettings.ClientUrl);
            }).AddTransientHttpErrorPolicy(builder =>
                builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(15),
                }));
        }
    }
}
