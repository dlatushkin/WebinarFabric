using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using ClusterModels.Lines;
using ClusterModels.Trains;
using Microsoft.ServiceFabric.Data.Collections;
using ServiceCommon;
using ServiceInterfaces;

namespace GpsPositionService
{
    public class GpsPositionService : BaseStatefulService, IGpsPositionService
    {
        private readonly ITimeSource _timeSource;

        public GpsPositionService(
            StatefulServiceContext context,
            ITimeSource timeSource)
            : base(context)
        {
            _timeSource = timeSource;
        }

        public async Task<TrainPositionMoment[]> GetTrainsPositionAsync(string lineId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var trainPositionMomentsDict = await GetMovementsDict();
                var lineMovements = await trainPositionMomentsDict.TryGetValueAsync(tx, lineId);
                if (lineMovements.HasValue)
                {
                    return lineMovements.Value;
                }
                else
                {
                    return new TrainPositionMoment[0];
                }
             }
        }

        public async Task ReceiveTrainsPositionAsync(TrainPosition[] trainPositions)
        {
            var now = _timeSource.GetNow();

            var trainPositionMoments = trainPositions.Select(tp => new TrainPositionMoment
            {
                LineId = tp.LineId,
                Number = tp.Number,
                Point = tp.Point,
                Moment = now
            }).ToArray();

            var movementsGrouped = trainPositionMoments.GroupBy(m => m.LineId);

            using (var tx = StateManager.CreateTransaction())
            {
                var trainPositionMomentsDict = await GetMovementsDict();

                foreach (var lineMovement in movementsGrouped)
                {
                    await trainPositionMomentsDict.AddOrUpdateAsync(
                        tx,
                        lineMovement.Key,
                        lineMovement.ToArray(),
                        (k, v) => lineMovement.ToArray());

                    await tx.CommitAsync();
                }
            }
        }

        private Task<IReliableDictionary<string, TrainPositionMoment[]>> GetMovementsDict() =>
            StateManager.GetOrAddAsync<IReliableDictionary<string, TrainPositionMoment[]>>("TrainPositionMoment");
    }
}
