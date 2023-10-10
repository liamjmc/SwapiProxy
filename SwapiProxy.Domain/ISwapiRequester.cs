namespace SwapiProxy.Domain
{
    public interface ISwapiRequester
    {
        Task<string> GetAsync(string relativeUrl);
    }
}