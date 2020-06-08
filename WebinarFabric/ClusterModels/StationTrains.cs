using ClusterModels.Lines;
using ClusterModels.Trains;

namespace ClusterModels
{
    public class StationTrains
    {
        public Station Station { get; set; }

        public Train[] Trains { get; set; }
    }
}
