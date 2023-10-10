using System.Net.Http;
using System.Text.Json;

namespace SwapiProxy.Domain
{
    public class AggregateSwapiRequester : IAggregateSwapiRequester
    {
        private readonly ISwapiRequester _swapiRequester;

        public AggregateSwapiRequester(ISwapiRequester swapiRequester)
        {
            _swapiRequester = swapiRequester;
        }

        public async Task<IEnumerable<object>> GetAsync(IEnumerable<string> relativeUrls)
        {
            var result = new List<object>();

            foreach (var relativeUrl in relativeUrls)
            {
                result.Add(await _swapiRequester.GetAsync(relativeUrl));
            }

            return result;
        }
    }
}   