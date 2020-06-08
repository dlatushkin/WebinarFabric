using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceCommon;
using WebModels;

namespace WebApiWebinarService.Controllers
{
    [ApiController]
    [Route("api/train-positions")]
    public class GpsPositionController : ControllerBase
    {
        private readonly IRemoteServices _remoteServices;

        public GpsPositionController(IRemoteServices remoteServices)
        {
            _remoteServices = remoteServices;
        }

        [HttpPost]
        public async Task<IActionResult> Post(TrainPositionEntry[] trainPositions)
        {
            var lines = await _remoteServices.TopologyService.GetLinesAsync();
            return Ok(lines);
        }
    }
}
