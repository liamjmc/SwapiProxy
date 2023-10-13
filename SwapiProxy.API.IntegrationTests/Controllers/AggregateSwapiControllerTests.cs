using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using SwapiProxy.API.IntegrationTests.Helpers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Proxy.Domain;
using Moq.Protected;
using Newtonsoft.Json;
using Moq;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace SwapiProxy.API.IntegrationTests.Controllers
{
    public class AggregateSwapiControllerTests
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(Enumerable.Empty<string>()))
                });

            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("https://starwarsapi.co/");

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(h => h.CreateClient("testClient")).Returns(httpClient);

            var webApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(httpClientFactoryMock.Object);
                });

                builder.UseEnvironment("Test");
            });
            _httpClient = webApplicationFactory.CreateDefaultClient();
        }

        [Test]
        public async Task GivenARequestToTheSwapiProxy_WhenABearerTokenIsNotGiven_ThenA401IsReturned()
        {
            var relativeUrls = new string[] { "films/2", "starships/3", "characters/4" };
            var requestBody = JsonConvert.SerializeObject(relativeUrls);

            var values = new Dictionary<string, string>
            {
                { "body", requestBody },
                { "Content-Type", "application/json" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await _httpClient.PostAsync("/api/v2/swapis", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}