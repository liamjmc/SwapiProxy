using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Proxy.Domain.UnitTests.Models;
using System.Net;
using Polly.RateLimit;
using Polly;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

namespace Proxy.Domain.UnitTests
{
    public class ProxyRequesterTests
    {
        private ProxyRequester _proxyRequester;
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<ILogger<ProxyRequester>> _loggerMock;
        private Mock<IRateLimiter> _rateLimiterMock;

        [SetUp]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _rateLimiterMock = new Mock<IRateLimiter>();
            _rateLimiterMock.Setup(r => r.GetPolicy()).Returns(Policy.RateLimitAsync(10, TimeSpan.FromSeconds(100)));

            _loggerMock = new Mock<ILogger<ProxyRequester>>();
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

            var appSettings = Options.Create(new AppSettings { ClientName = "swapi-client", ClientUrl = "https://client.com/" });

            _proxyRequester = new ProxyRequester(_rateLimiterMock.Object, _httpClientFactoryMock.Object, appSettings, _loggerMock.Object);

            var result = await _proxyRequester.GetAsync(relativeUrl, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
        }

        [TestCase(" ")]
        [TestCase("")]
        [TestCase(null)]
        public void GivenThatSwapiIsRequested_WhenTheRelativeUrlIsEmpty_ThenAnErrorIsThrown(string relativeUrl)
        {
            var film = new Film();

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

            var appSettings = Options.Create(new AppSettings { ClientName = "swapi-client", ClientUrl = "https://client.com/" });

            _proxyRequester = new ProxyRequester(_rateLimiterMock.Object, _httpClientFactoryMock.Object, appSettings, _loggerMock.Object);

            Assert.That(async () => await _proxyRequester.GetAsync(relativeUrl, CancellationToken.None), Throws.ArgumentNullException);
        }

        [TestCase(" ")]
        [TestCase("")]
        [TestCase(null)]
        public void GivenThatSwapiIsRequested_WhenTheClientNameIsEmpty_ThenAnErrorIsThrown(string clientName)
        {
            var film = new Film();

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

            var appSettings = Options.Create(new AppSettings { ClientName = clientName, ClientUrl = "https://client.com/" });

            _proxyRequester = new ProxyRequester(_rateLimiterMock.Object, _httpClientFactoryMock.Object, appSettings, _loggerMock.Object);

            Assert.That(async () => await _proxyRequester.GetAsync("/films/2", CancellationToken.None), Throws.ArgumentNullException);
        }

        [TestCase(" ")]
        [TestCase("")]
        [TestCase(null)]
        public void GivenThatSwapiIsRequested_WhenTheClientUrlIsEmpty_ThenAnErrorIsThrown(string clientUrl)
        {
            var film = new Film();

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

            var appSettings = Options.Create(new AppSettings { ClientName = "swapi-client", ClientUrl = clientUrl });

            _proxyRequester = new ProxyRequester(_rateLimiterMock.Object, _httpClientFactoryMock.Object, appSettings, _loggerMock.Object);

            Assert.That(async () => await _proxyRequester.GetAsync("/films/2", CancellationToken.None), Throws.ArgumentNullException);
        }

        [Test]
        public async Task GivenThatSwapiIsRequested_WhenTheApiReturnsAnError_ThenAnErrorIsReturned()
        {
            var film = new Film();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(string.Empty)
                });

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("https://starwarsapi.co/");

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var appSettings = Options.Create(new AppSettings { ClientName = "swapi-client", ClientUrl = "https://client.com/" });

            _proxyRequester = new ProxyRequester(_rateLimiterMock.Object, _httpClientFactoryMock.Object, appSettings, _loggerMock.Object);

            var result = await _proxyRequester.GetAsync("/films/4", CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}