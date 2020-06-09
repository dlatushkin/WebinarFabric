using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceCommon;

namespace GpsPositionService
{
    internal static class Program
    {
        private static void Main()
        {
            new ServiceRunnerGpsPosition().Run<GpsPositionService>("GpsPositionServiceType", ServiceEventSource.Current);
        }

        private class ServiceRunnerGpsPosition : BaseServiceRunner
        {
            protected override void RegisterTypeSpecific(ContainerBuilder builder)
            {
            }

            protected override void RunSpecific(string typeName)
            {
                RunStatefulService<GpsPositionService>(typeName);
            }
        }
    }
}
