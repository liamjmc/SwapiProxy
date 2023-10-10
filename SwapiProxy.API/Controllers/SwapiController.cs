using Microsoft.AspNetCore.Mvc;
using SwapiProxy.Domain;

namespace SwapiProxy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SwapiController : ControllerBase
    {
        private readonly ISwapiRequester _swapiRequester;
        private readonly ILogger<SwapiController> _logger;

        public SwapiController(ISwapiRequester swapiRequester, ILogger<SwapiController> logger)
        {
            _swapiRequester = swapiRequester;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get([FromQuery] string relativeUrl)
        {
            return await _swapiRequester.GetAsync(relativeUrl);
        }
    }
}