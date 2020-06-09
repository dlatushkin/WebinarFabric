using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{lineId}/stations")]
        public async Task<IActionResult> Get(string lineId)
        {
            var stations = await _remoteServices.TopologyService.GetStationsAsync(lineId);
            return Ok(stations);
        }
    }
}
