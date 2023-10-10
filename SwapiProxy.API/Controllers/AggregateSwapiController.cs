using Microsoft.AspNetCore.Mvc;
using SwapiProxy.Domain;

namespace SwapiProxy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AggregateSwapiController : ControllerBase
    {
        private readonly IAggregateSwapiRequester _aggregateSwapiRequester;
        private readonly ILogger<SwapiController> _logger;

        public AggregateSwapiController(IAggregateSwapiRequester aggregateSwapiRequester, ILogger<SwapiController> logger)
        {
            _aggregateSwapiRequester = aggregateSwapiRequester;
            _logger = logger;
        }

        //TODO: Shouldn't be a POST but aggregating a number of requests takes a body.
        // Potentially a base64 encoded query string?
        [HttpPost(Name = "GetAggregateSwapiResponse")]
        public async Task<object> Get([FromBody] IEnumerable<string> relativeUrls)
        {
            return await _aggregateSwapiRequester.GetAsync(relativeUrls);
        }
    }
}