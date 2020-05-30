using System.Threading.Tasks;
using ClusterModels.Trains;

namespace ServiceInterfaces
{
    public interface ITrainService
    {
        Task<Train[]> GetTrainsAsync();
    }
}
