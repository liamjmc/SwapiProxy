using Polly.RateLimit;

namespace SwapiProxy.Domain
{
    public interface IRateLimiter
    {
        AsyncRateLimitPolicy GetPolicy();
    }
}