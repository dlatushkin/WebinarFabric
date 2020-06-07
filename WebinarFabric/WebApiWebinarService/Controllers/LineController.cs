using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiWebinarService.Controllers
{
    [ApiController]
    [Route("api/lines")]
    public class LineController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public LineController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(Ok());
        }
    }
}
