using Moq;

namespace SwapiProxy.Domain.UnitTests
{
    public class AggregateSwapiRequesterTests
    {
        private AggregateSwapiRequester _aggregateSwapiRequester;
        private Mock<ISwapiRequester> _swapiRequesterMock;

        [SetUp]
        public void Setup()
        {
            _swapiRequesterMock = new Mock<ISwapiRequester>();
            _aggregateSwapiRequester = new AggregateSwapiRequester(_swapiRequesterMock.Object);
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

        //TODO: Need to consider what is best to return in the zero instance
        [Test]
        public async Task GivenAnAggregateSwapiRequestIsMade_WhenTheRequestHasNoRelativeUrls_ThenNoResultIsReturned()
        {
            _swapiRequesterMock.Setup(s => s.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new object());

            var result = await _aggregateSwapiRequester.GetAsync(Enumerable.Empty<string>());

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GivenAnAggregateSwapiRequestIsMade_WhenTheSwapiRequestThrowsAnError_ThenAnErrorIsThrown()
        {
            var relativeUrls = Enumerable.Range(0, 10).Select(x => Guid.NewGuid().ToString());

            _swapiRequesterMock.Setup(s => s.GetAsync(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            Assert.That(async () => await _aggregateSwapiRequester.GetAsync(relativeUrls), Throws.ArgumentException);
        }
    }
}