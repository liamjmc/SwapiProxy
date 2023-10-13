using Moq;
using Newtonsoft.Json.Linq;

namespace Proxy.Domain.UnitTests
{
    public class AggregateProxyRequesterTests
    {
        private AggregateProxyRequester _aggregateSwapiRequester;
        private Mock<IProxyRequester> _swapiRequesterMock;

        [SetUp]
        public void Setup()
        {
            _swapiRequesterMock = new Mock<IProxyRequester>();
            _aggregateSwapiRequester = new AggregateProxyRequester(_swapiRequesterMock.Object);
        }

        [TestCase(1)]
        [TestCase(6)]
        [TestCase(17)]
        [TestCase(36)]
        public async Task GivenAnAggregateSwapiRequestIsMade_WhenTheRequestHasCorrectRelativeUrls_ThenAResultIsReturned(int numberOfRows)
        {
            var relativeUrls = Enumerable.Range(0, 10).Select(x => Guid.NewGuid().ToString());

            _swapiRequesterMock.Setup(s => s.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new object());

            var result = await _aggregateSwapiRequester.GetAsync(relativeUrls);

            Assert.That(result.Count(), Is.EqualTo(relativeUrls.Count()));
        }

        [Test]
        public void GivenAnAggregateSwapiRequestIsMade_WhenTheRequestHasNoRelativeUrls_ThenErrorIsThrown()
        {
            _swapiRequesterMock.Setup(s => s.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new object());

            Assert.That(async () => await _aggregateSwapiRequester.GetAsync(Enumerable.Empty<string>()), Throws.ArgumentException);
        }

        [Test]
        public void GivenAnAggregateSwapiRequestIsMade_WhenTheSwapiRequestThrowsAnError_ThenAnErrorIsThrown()
        {
            var relativeUrls = Enumerable.Range(0, 10).Select(x => Guid.NewGuid().ToString());

            _swapiRequesterMock.Setup(s => s.GetAsync(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            Assert.That(async () => await _aggregateSwapiRequester.GetAsync(relativeUrls), Throws.ArgumentException);
        }
    }
}