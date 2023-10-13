namespace Proxy.Domain
{
    public interface IAggregateProxyRequester
    {
        Task<IEnumerable<object>> GetAsync(IEnumerable<string> relativeUrls, CancellationToken cancellationToken);
    }
}