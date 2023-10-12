using Microsoft.AspNetCore.Mvc;
using Proxy.Domain;

namespace SwapiProxy.API.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "V2")]
    [Route("api/v{version:apiVersion}/swapis")]
    public class AggregateSwapiController : ControllerBase
    {
        private readonly IAggregateProxyRequester _aggregateSwapiRequester;
        private readonly ILogger<SwapiController> _logger;

        public AggregateSwapiController(IAggregateProxyRequester aggregateSwapiRequester, ILogger<SwapiController> logger)
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