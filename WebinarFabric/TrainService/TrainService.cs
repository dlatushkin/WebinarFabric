using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using ClusterModels;
using ClusterModels.Trains;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceCommon;
using ServiceInterfaces;
using HelpersCommon;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace TrainService
{
    public class TrainService : StatelessService, ITrainService
    {
        private readonly IRemoteServices _remoteServices;

        public TrainService(
            StatelessServiceContext context,
            IRemoteServices remoteServices)
            : base(context)
        {
            _remoteServices = remoteServices;
        }

        public async Task<StationTrains[]> GetStationTrains()
        {
            var result = new List<StationTrains>();

            var lines = await _remoteServices.TopologyService.GetLinesAsync();
            foreach (var line in lines)
            {
                var trainPositionMoments = await _remoteServices.GpsPositionService.GetTrainsPositionAsync(line.Id);

                var lineTrainPositionMoments = trainPositionMoments.Where(tpm => line.Id.EqualsOrdinalIgnoreCase(tpm.LineId));

                var stations = await _remoteServices.TopologyService.GetStationsAsync(line.Id);

                foreach (var station in stations)
                {
                    var trains = trainPositionMoments
                        .Where(tpm => station.FromPoint <= tpm.Point && tpm.Point <= station.ToPoint)
                        .ToArray();

                    var stationTrains = new StationTrains
                    {
                        Station = station,
                        Trains = trains.Select(t => new Train { Number = t.Number }).ToArray()
                    };

                    result.Add(stationTrains);
                }
            }

            return result.ToArray();
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
    }
}
