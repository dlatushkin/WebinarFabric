using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClusterModels.Lines;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceCommon;
using ServiceInterfaces;
using HelpersCommon;
using TopologyService.Logics;

namespace TopologyService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public class TopologyService : BaseStatefulService, ITopologyService
    {
        private readonly ITopologyLogic _topologyLogic;

        public TopologyService(
            ITopologyLogic topologyLogic,
            StatefulServiceContext context)
            : base(context)
        {
            _topologyLogic = topologyLogic;
        }

        protected override async Task RunSpecificAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _topologyLogic.RefreshAsync();

                await Task.Delay(10000);
            }
        }

        public Task<Line[]> GetLinesAsync()
        {
            var lines = _topologyLogic.Topology.Keys.ToArray();
            return Task.FromResult(lines);
        }

        public Task<Station[]> GetStationsAsync(string lineId)
        {
            var topology = _topologyLogic.Topology;

            var line = topology.Keys.FirstOrDefault(k => lineId.EqualsOrdinalIgnoreCase(k.Id));
            if (line == default)
            {
                return Task.FromException<Station[]>(new InvalidOperationException($"Couldn't find line by id='{lineId}'"));
            }

            var lines = topology[line];
            return Task.FromResult(lines);
        }
    }
}
