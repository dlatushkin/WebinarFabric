using ClusterModels.Lines;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyService.Logics
{
    public class TopologyLogic : ITopologyLogic
    {
        public class TopologyRecord
        {
            public Line Line { get; set; }

            public Station[] Stations { get; set; }
        }


        private readonly TopologyConfig _topologyConfig;

        private Dictionary<Line, Station[]> _topology = new Dictionary<Line, Station[]>();

        public TopologyLogic(TopologyConfig topologyConfig)
        {
            _topologyConfig = topologyConfig;
        }

        public Dictionary<Line, Station[]> Topology => _topology;

        public async Task RefreshAsync()
        {
            try
            {
                var responseRecords = await _topologyConfig.Url
                            .AppendPathSegment("topology")
                            .GetJsonAsync<TopologyRecord[]>();

                _topology = responseRecords.ToDictionary(r => r.Line, r => r.Stations);
            }
            catch (FlurlHttpException fex)
            {
            }
        }
    }
}
