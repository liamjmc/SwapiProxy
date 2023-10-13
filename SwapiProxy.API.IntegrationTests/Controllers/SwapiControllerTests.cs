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

namespace SwapiProxy.API.IntegrationTests.Controllers
{
    public class SwapiControllerTests
    {
        //TODO: currently getting the jwt details from the config
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
        public async Task GivenARequestToTheSwapiProxy_WhenABearerTokenIsNotGiven_ThenA404IsReturned()
        {
            var response = await _httpClient.GetAsync($"/api/v1/swapi/films/2");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GivenARequestToTheSwapiProxy_WhenABearerTokenIsGiven_ThenA404IsReturned()
        {
            var bearerToken = BearerTokenHelper.GetBearerToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.GetAsync($"/api/v1/swapi/films/2");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}