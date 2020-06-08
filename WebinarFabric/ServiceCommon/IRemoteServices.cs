using ServiceInterfaces;

namespace ServiceCommon
{
    public interface IRemoteServices
    {
        ITopologyService TopologyService { get; }

        ITrainService TrainService { get; }

        IBoardingService BoardingService { get; }

        IGpsPositionService WagonGpsService { get; }
    }
}
