using System.Threading.Tasks;
using WebinarModels.Lines;

namespace WebinarServiceInterfaces
{
    public interface ITopologyService
    {
        Task<Line[]> GetLinesAsync();

        Task<Station[]> GetStationsAsync(string lineId);
    }
}
