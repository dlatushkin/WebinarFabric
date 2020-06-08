using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceCommon;
using WebModels;

namespace WebApiWebinarService.Controllers
{
    [ApiController]
    [Route("api/station-train")]
    public class TrainController : ControllerBase
    {
        private readonly IRemoteServices _remoteServices;

        public TrainController(IRemoteServices remoteServices)
        {
            _remoteServices = remoteServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var trainModels = await _remoteServices.TrainService.GetStationTrains();

            var trainEntries = trainModels
                .Select(st => new StationTrainsEntry
                {
                    Station = new StationEntry
                    {
                        Code = st.Station.Code,
                        Name = st.Station.Name
                    },
                    Trains = st.Trains.Select(t => new TrainEntry { Number = t.Number }).ToArray()
                })
                .ToArray();

            return Ok(trainEntries);
        }
    }
}
