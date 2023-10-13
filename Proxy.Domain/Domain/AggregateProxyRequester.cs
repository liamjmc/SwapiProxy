namespace Proxy.Domain
{
    public class AggregateProxyRequester : IAggregateProxyRequester
    {
        private readonly IProxyRequester _swapiRequester;

        public AggregateProxyRequester(IProxyRequester swapiRequester)
        {
            _swapiRequester = swapiRequester;
        }

        public async Task<IEnumerable<object>> GetAsync(IEnumerable<string> relativeUrls, CancellationToken cancellationToken)
        {
            if (relativeUrls.Any() == false)
                throw new ArgumentException($"No relative URLs have been given.");

            var swapiRequestTasks = relativeUrls.Select(r => _swapiRequester.GetAsync(r, cancellationToken)).ToList();

            var result = await Task.WhenAll(swapiRequestTasks);

            return result?.ToList();
        }
    }
}   