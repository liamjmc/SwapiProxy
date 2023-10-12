using Polly.RateLimit;
using Polly;

namespace SwapiProxy.Domain
{
    public class RateLimiter : IRateLimiter
    {
        // 5 actions within 10 seconds, 2 burst
        public AsyncRateLimitPolicy GetPolicy() => Policy.RateLimitAsync(5, TimeSpan.FromSeconds(10), 2);
    }
}
