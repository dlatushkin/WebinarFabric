using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using ServiceInterfaces;

namespace ServiceCommon
{
    public class RemoteServices : IRemoteServices
    {
        private const string ListenerName = "V2_1Listener";

        private readonly Lazy<ITopologyService> _topologyService;
        private readonly Lazy<IGpsPositionService> _gpsPositionService;
        private readonly Lazy<ITrainService> _trainService;

        public RemoteServices()
        {
            var applicationNamePrefix = $"{Environment.GetEnvironmentVariable("Fabric_ApplicationName")}/";

            var topologyServiceUrl = applicationNamePrefix + "TopologyService";
            _topologyService =
                new Lazy<ITopologyService>(
                    () => ServiceProxy.Create<ITopologyService>(
                        new Uri(topologyServiceUrl),
                        new ServicePartitionKey(0),
                        listenerName: ListenerName));

            var gpsPositionServiceUrl = applicationNamePrefix + "GpsPositionService";
            _gpsPositionService =
                new Lazy<IGpsPositionService>(
                    () => ServiceProxy.Create<IGpsPositionService>(
                        new Uri(gpsPositionServiceUrl),
                        new ServicePartitionKey(0),
                        listenerName: ListenerName));

            var trainServiceUrl = applicationNamePrefix + "TrainService";
            _trainService =
                new Lazy<ITrainService>(
                    () => ServiceProxy.Create<ITrainService>(
                        new Uri(trainServiceUrl),
                        new ServicePartitionKey(0),
                        listenerName: ListenerName));
        }

        public ITopologyService TopologyService => _topologyService.Value;

        public ITrainService TrainService => _trainService.Value;

        public IGpsPositionService GpsPositionService => _gpsPositionService.Value;
    }
}
