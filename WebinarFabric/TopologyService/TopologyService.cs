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

namespace TopologyService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class TopologyService : BaseStatefulService, ITopologyService
    {
        private readonly Dictionary<Line, Station[]> _lineStations = new Dictionary<Line, Station[]>();

        public TopologyService(StatefulServiceContext context)
            : base(context)
        { }

        protected override Task RunSpecificAsync(CancellationToken cancellationToken)
        {
            return base.RunSpecificAsync(cancellationToken);
        }

        public Task<Line[]> GetLinesAsync()
        {
            var lines = _lineStations.Keys.ToArray();
            return Task.FromResult(lines);
        }

        public Task<Station[]> GetStationsAsync(string lineId)
        {
            var lineStations = _lineStations;

            var line = lineStations.Keys.FirstOrDefault(k => lineId.EqualsOrdinalIgnoreCase(k.Id));
            if (line == default)
            {
                return Task.FromException<Station[]>(new InvalidOperationException($"Couldn't find line by id='{lineId}'"));
            }

            var lines = lineStations[line];
            return Task.FromResult(lines);
        }
    }
}
