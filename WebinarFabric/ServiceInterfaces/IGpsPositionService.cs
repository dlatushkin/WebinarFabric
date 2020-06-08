using System.Threading.Tasks;
using ClusterModels.Trains;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ServiceInterfaces
{
    public interface IGpsPositionService : IService
    {
        Task<TrainPosition[]> GetTrainsPositionAsync();

        Task ReceiveTrainsPositionAsync(TrainPosition[] trainPositions);
    }
}
