using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceCommon;

namespace TrainService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            new ServiceRunnerTrain().Run<TrainService>("TrainServiceType", ServiceEventSource.Current);
            //try
            //{
            //    // The ServiceManifest.XML file defines one or more service type names.
            //    // Registering a service maps a service type name to a .NET type.
            //    // When Service Fabric creates an instance of this service type,
            //    // an instance of the class is created in this host process.

            //    ServiceRuntime.RegisterServiceAsync("TrainServiceType",
            //        context => new TrainService(context)).GetAwaiter().GetResult();

            //    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(TrainService).Name);

            //    // Prevents this host process from terminating so services keep running.
            //    Thread.Sleep(Timeout.Infinite);
            //}
            //catch (Exception e)
            //{
            //    ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
            //    throw;
            //}
        }

        private class ServiceRunnerTrain : BaseServiceRunner
        {
            protected override void RegisterTypeSpecific(ContainerBuilder builder)
            {
            }

            protected override void RunSpecific(string typeName)
            {
                RunStatelessService<TrainService>(typeName);
            }
        }
    }
}
