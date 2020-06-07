﻿using System.Threading.Tasks;
using ClusterModels.Trains;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ServiceInterfaces
{
    public interface ITrainService : IService
    {
        Task<Train[]> GetTrainsAsync();
    }
}
