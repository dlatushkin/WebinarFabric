using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceCommon;
using TopologyService.Logics;

namespace TopologyService
{
    internal static class Program
    {
        private static void Main()
        {
            new ServiceRunnerTopology().Run<TopologyService>("TopologyServiceType", ServiceEventSource.Current);
        }

        private class ServiceRunnerTopology : BaseServiceRunner
        {
            protected override void RegisterTypeSpecific(ContainerBuilder builder)
            {
                builder.RegisterType<TopologyConfig>().InstancePerLifetimeScope();
                builder.RegisterType<TopologyLogic>().As<ITopologyLogic>().InstancePerLifetimeScope();
            }

            protected override void RunSpecific(string typeName)
            {
                RunStatefulService<TopologyService>(typeName);
            }
        }
    }
}
