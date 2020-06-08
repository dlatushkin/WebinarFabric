using System.Fabric;
using System.Threading.Tasks;
using ClusterModels.Trains;
using ServiceCommon;
using ServiceInterfaces;

namespace GpsPositionService
{
    public class GpsPositionService : BaseStatefulService, IGpsPositionService
    {
        private TrainPosition[] _trainPositions = new TrainPosition[0];

        public GpsPositionService(StatefulServiceContext context)
            : base(context)
        { }

        public Task<TrainPosition[]> GetTrainsPositionAsync() => Task.FromResult(_trainPositions);

        public Task ReceiveTrainsPositionAsync(TrainPosition[] trainPositions)
        {
            _trainPositions = trainPositions;
            return Task.CompletedTask;
        }
    }
}
