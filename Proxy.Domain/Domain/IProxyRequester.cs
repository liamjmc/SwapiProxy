namespace Proxy.Domain
{
    public interface IProxyRequester
    {
        Task<object?> GetAsync(string relativeUrl);
    }
}