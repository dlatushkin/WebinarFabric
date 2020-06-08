using System.Linq;
using System.Threading.Tasks;
using ClusterModels.Trains;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var trainModels = await _remoteServices.GpsPositionService.GetTrainsPositionAsync();

            var trainEntries = trainModels
                .Select(t => new TrainPositionEntry { LineId = t.LineId, Number = t.Number, Point = t.Point })
                .ToArray();

            return Ok(trainEntries);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TrainPositionEntry[] trainPositions)
        {
            var trainModels = trainPositions
                .Select(t => new TrainPosition { LineId = t.LineId, Number = t.Number, Point = t.Point })
                .ToArray();

            await _remoteServices.GpsPositionService.ReceiveTrainsPositionAsync(trainModels);
            return Accepted();
        }
    }
}
