namespace SwapiProxy.Domain
{
    public interface ISwapiRequester
    {
        Task<object> GetAsync(string relativeUrl);
    }
}