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
            if (relativeUrls.Any() == false)
                throw new ArgumentException($"No relative URLs have been given.");

            var result = new List<object>();

            foreach (var relativeUrl in relativeUrls)
            {
                var response = await _swapiRequester.GetAsync(relativeUrl);

                if (response != null)
                    result.Add(response);
            }

            return result;
        }
    }
}   