using Polly.RateLimit;
using Polly;

namespace Proxy.Domain
{
    public class RateLimiter : IRateLimiter
    {
        // 5 actions within 10 seconds, 2 burst
        public AsyncRateLimitPolicy GetPolicy() => Policy.RateLimitAsync(50, TimeSpan.FromSeconds(1), 20);
    }
}
