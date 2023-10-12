using Polly.RateLimit;

namespace Proxy.Domain
{
    public interface IRateLimiter
    {
        AsyncRateLimitPolicy GetPolicy();
    }
}