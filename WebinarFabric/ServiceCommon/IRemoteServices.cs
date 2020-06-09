using ServiceInterfaces;

namespace ServiceCommon
{
    public interface IRemoteServices
    {
        ITopologyService TopologyService { get; }

        ITrainService TrainService { get; }

        IGpsPositionService GpsPositionService { get; }
    }
}
