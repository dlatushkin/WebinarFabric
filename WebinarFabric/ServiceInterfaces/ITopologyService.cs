using System.Threading.Tasks;
using ClusterModels.Lines;

namespace ServiceInterfaces
{
    public interface ITopologyService
    {
        Task<Line[]> GetLinesAsync();

        Task<Station[]> GetStationsAsync(string lineId);
    }
}
