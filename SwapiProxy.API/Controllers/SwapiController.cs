using Microsoft.AspNetCore.Mvc;
using SwapiProxy.Domain;

namespace SwapiProxy.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "V1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SwapiController : ControllerBase
    {
        private readonly ISwapiRequester _swapiRequester;
        private readonly ILogger<SwapiController> _logger;

        public SwapiController(ISwapiRequester swapiRequester, ILogger<SwapiController> logger)
        {
            _swapiRequester = swapiRequester;
            _logger = logger;
        }

        [HttpGet("{*relativeUrl}", Name = "GetSwapiResponse")]
        public async Task<object> Get(string relativeUrl)
        {
            return await _swapiRequester.GetAsync(relativeUrl);
        }
    }
}