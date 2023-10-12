using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NuGet.Frameworks;
using Proxy.Domain.UnitTests.Models;
using System.Net;

namespace Proxy.Domain.UnitTests
{
    public class SwapiRequesterTests
    {
        private ProxyRequester _swapiRequester;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var rateLimiter = new Mock<IRateLimiter>();

            _swapiRequester = new ProxyRequester(rateLimiter.Object, _httpClientFactoryMock.Object);
        }

        [TestCase("films/1")]
        [TestCase("/planets/1")]
        [TestCase("starships/1/")]
        [TestCase("/starships/3/")]
        [TestCase("/starships/3?search=lk3m22")]
        public async Task GivenThatSwapiIsRequested_WhenTheRelativeUrlIsValid_ThenAResultIsReturned(string relativeUrl)
        {
            var film = new Film
            {
                title = "The Empire Strikes Back",
                episode_id = 5,
                opening_crawl = "It is a dark time for the\r\nRebellion. Although the Death\r\nStar has been destroyed,\r\nImperial troops have driven the\r\nRebel forces from their hidden\r\nbase and pursued them across\r\nthe galaxy.\r\n\r\nEvading the dreaded Imperial\r\nStarfleet, a group of freedom\r\nfighters led by Luke Skywalker\r\nhas established a new secret\r\nbase on the remote ice world\r\nof Hoth.\r\n\r\nThe evil lord Darth Vader,\r\nobsessed with finding young\r\nSkywalker, has dispatched\r\nthousands of remote probes into\r\nthe far reaches of space....",
                director = "Irvin Kershner",
                producer = "Gary Kurtz, Rick McCallum",
                characters = new List<string> {
                    "https =//swapi.dev/api/people/1/",
                    "https =//swapi.dev/api/people/2/"
                },
                planets = new List<string> {
                    "https =//swapi.dev/api/planets/4/"
                },
                starships = new List<string> {

                    "https =//swapi.dev/api/starships/3/",
                    "https =//swapi.dev/api/starships/10/"
                },
                vehicles = new List<string> {
                    "https =//swapi.dev/api/vehicles/8/"
                },
                species = new List<string> {
                    "https =//swapi.dev/api/species/1/"
                },
                url = "https =//swapi.dev/api/films/2/"
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(film))
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("https://starwarsapi.co/");

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = await _swapiRequester.GetAsync(relativeUrl);

            Assert.That(result, Is.Not.Null);
        }
    }
}