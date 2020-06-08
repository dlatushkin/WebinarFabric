using System.Threading.Tasks;
using ClusterModels;
using ClusterModels.Trains;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ServiceInterfaces
{
    public interface IGpsPositionService : IService
    {
        Task<TrainPositionMoment[]> GetTrainsPositionAsync();

        Task ReceiveTrainsPositionAsync(TrainPosition[] trainPositions);
    }
}
