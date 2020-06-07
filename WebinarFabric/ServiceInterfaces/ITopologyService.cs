using System.Threading.Tasks;
using ClusterModels.Lines;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ServiceInterfaces
{
    public interface ITopologyService : IService
    {
        Task<Line[]> GetLinesAsync();

        Task<Station[]> GetStationsAsync(string lineId);
    }
}
