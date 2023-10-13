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

        /*
         * Post request used in order to utilise a body of aggregated relative URLs even though this is essentially a GET
         * Could possible use other methods such as a base64 encoded query string value
         */
        [HttpPost(Name = "GetAggregateSwapiResponse")]
        public async Task<ActionResult<IEnumerable<object>>> Get([FromBody] IEnumerable<string> relativeUrls, CancellationToken cancellationToken)
        {
            var result = await _aggregateSwapiRequester.GetAsync(relativeUrls, cancellationToken);

            return Ok(result);
        }
    }
}