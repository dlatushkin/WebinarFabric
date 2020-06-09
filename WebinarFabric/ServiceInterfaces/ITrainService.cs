using System.Threading.Tasks;
using ClusterModels;
using ClusterModels.Trains;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ServiceInterfaces
{
    public interface ITrainService : IService
    {
        Task<StationTrains[]> GetStationTrains();
    }
}
