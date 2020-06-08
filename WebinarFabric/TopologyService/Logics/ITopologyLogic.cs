using System.Collections.Generic;
using System.Threading.Tasks;
using ClusterModels.Lines;

namespace TopologyService.Logics
{
    public interface ITopologyLogic
    {
        Dictionary<Line, Station[]> Topology { get; }

        Task RefreshAsync();
    }
}
