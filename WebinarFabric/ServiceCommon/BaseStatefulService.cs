using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ServiceCommon
{
    public abstract class BaseStatefulService : StatefulService, IService
    {
        protected BaseStatefulService(StatefulServiceContext serviceContext) : base(serviceContext)
        {
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            var listeners = this.CreateServiceRemotingReplicaListeners();
            return listeners;
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                await RunSpecificAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected virtual Task RunSpecificAsync(CancellationToken cancellationToken) => Task.FromResult(true);
    }
}
