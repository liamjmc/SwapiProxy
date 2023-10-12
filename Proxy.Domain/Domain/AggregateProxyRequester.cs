namespace Proxy.Domain
{
    public class AggregateProxyRequester : IAggregateProxyRequester
    {
        private readonly IProxyRequester _swapiRequester;

        public AggregateProxyRequester(IProxyRequester swapiRequester)
        {
            _swapiRequester = swapiRequester;
        }

        public async Task<IEnumerable<object>> GetAsync(IEnumerable<string> relativeUrls)
        {
            var result = new List<object>();

            foreach (var relativeUrl in relativeUrls)
            {
                result.Add(await _swapiRequester.GetAsync(relativeUrl));
            }

            return result;
        }
    }
}   