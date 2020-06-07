using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Integration.ServiceFabric;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ServiceCommon
{
    public abstract class BaseServiceRunner
    {
        private IServiceEventSource _eventSource;

        public virtual void Run<T>(string serviceTypeName, IServiceEventSource serviceEventSourceBase)
        {
            _eventSource = serviceEventSourceBase;
            //ILogger logger = null;
            try
            {
                //logger = LoggerFactoryCreator.GetLoggerFactory().Create($"Initialization.{typeof(T).Name}");
                RunSpecific(serviceTypeName);
            }
            catch (Exception ex)
            {
                serviceEventSourceBase.ServiceHostInitializationFailed(ex.ToString());
                //logger?.Error("Failed to initialize service", e);
                throw;
            }
        }

        protected void RunStatelessService<T>(string typeName) where T : StatelessService
        {
            var builder = GetContainerBuilder<T>();
            builder.RegisterStatelessService<T>(typeName).WithAttributeFiltering();
            BuildContainer<T>(builder);
        }

        protected void RunStatefulService<T>(string typeName) where T : StatefulService
        {
            var builder = GetContainerBuilder<T>();
            builder.RegisterStatefulService<T>(typeName).WithAttributeFiltering();
            BuildContainer<T>(builder);
        }

        protected abstract void RegisterTypeSpecific(ContainerBuilder builder);

        protected abstract void RunSpecific(string typeName);

        private void BuildContainer<T>(ContainerBuilder builder)
        {
            using (var container = builder.Build())
            {
                _eventSource?.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(T).Name);
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private ContainerBuilder GetContainerBuilder<T>()
        {
            var builder = new ContainerBuilder();

            builder.RegisterServiceFabricSupport();
            builder.RegisterType<TimeSource>().As<ITimeSource>();
            builder.RegisterType<RemoteServices>().As<IRemoteServices>();

            RegisterTypeSpecific(builder);

            return builder;
        }
    }
}
