using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using ClusterModels.Trains;
using ServiceCommon;
using ServiceInterfaces;

namespace GpsPositionService
{
    public class GpsPositionService : BaseStatefulService, IGpsPositionService
    {
        private readonly ITimeSource _timeSource;

        private TrainPositionMoment[] _trainPositionMoments = new TrainPositionMoment[0];

        public GpsPositionService(
            StatefulServiceContext context,
            ITimeSource timeSource)
            : base(context)
        {
            _timeSource = timeSource;
        }

        public Task<TrainPositionMoment[]> GetTrainsPositionAsync() => Task.FromResult(_trainPositionMoments);

        public Task ReceiveTrainsPositionAsync(TrainPosition[] trainPositions)
        {
            var now = _timeSource.GetNow();

            _trainPositionMoments = trainPositions.Select(tp => new TrainPositionMoment
            {
                LineId = tp.LineId,
                Number = tp.Number,
                Point = tp.Point,
                Moment = now
            }).ToArray();

            return Task.CompletedTask;
        }
    }
}
