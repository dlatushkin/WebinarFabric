using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCommon
{
    public class RemoteServices : IRemoteServices
    {
        private const string ListenerName = "V2_1Listener";

        private readonly Lazy<ITopologyService> _topologyService;

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
        }

        public ITopologyService TopologyService => _topologyService.Value;

        public ITrainService TrainService => throw new NotImplementedException();

        public IBoardingService BoardingService => throw new NotImplementedException();

        public IGpsPositionService WagonGpsService => throw new NotImplementedException();
    }
}
