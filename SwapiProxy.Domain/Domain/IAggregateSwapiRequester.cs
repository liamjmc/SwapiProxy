namespace SwapiProxy.Domain
{
    public interface IAggregateSwapiRequester
    {
        Task<IEnumerable<object>> GetAsync(IEnumerable<string> relativeUrls);
    }
}