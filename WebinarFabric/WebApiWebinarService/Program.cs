using Autofac;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceCommon;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiWebinarService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            new ServiceRunnerWebApiWebinar().Run<WebApiWebinarService>("WebApiWebinarServiceType", ServiceEventSource.Current);

            //try
            //{
            //    // The ServiceManifest.XML file defines one or more service type names.
            //    // Registering a service maps a service type name to a .NET type.
            //    // When Service Fabric creates an instance of this service type,
            //    // an instance of the class is created in this host process.

            //    ServiceRuntime.RegisterServiceAsync("WebApiWebinarServiceType",
            //        context => new WebApiWebinarService(context)).GetAwaiter().GetResult();

            //    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(WebApiWebinarService).Name);

            //    // Prevents this host process from terminating so services keeps running. 
            //    Thread.Sleep(Timeout.Infinite);
            //}
            //catch (Exception e)
            //{
            //    ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
            //    throw;
            //}
        }

        private class ServiceRunnerWebApiWebinar : BaseServiceRunner
        {
            protected override void RegisterTypeSpecific(ContainerBuilder builder)
            {
            }

            protected override void RunSpecific(string typeName)
            {
                RunStatelessService<WebApiWebinarService>(typeName);
            }
        }
    }
}
