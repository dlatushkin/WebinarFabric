using System;
using System.Fabric;
using System.Threading.Tasks;
using ClusterModels.Trains;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceInterfaces;

namespace TrainService
{
    public class TrainService : StatelessService, ITrainService
    {
        public TrainService(StatelessServiceContext context)
            : base(context)
        { }

        public Task<Train[]> GetTrainsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
