using System.Threading.Tasks;
using WebinarModels.Trains;

namespace WebinarServiceInterfaces
{
    public interface ITrainService
    {
        Task<Train[]> GetTrainsAsync();
    }
}
