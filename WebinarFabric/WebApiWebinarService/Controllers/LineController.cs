using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceCommon;

namespace WebApiWebinarService.Controllers
{
    [ApiController]
    [Route("api/lines")]
    public class LineController : ControllerBase
    {
        private readonly IRemoteServices _remoteServices;

        public LineController(IRemoteServices remoteServices)
        {
            _remoteServices = remoteServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lines = await _remoteServices.TopologyService.GetLinesAsync();
            return Ok(lines);
        }
    }
}
