using System;

namespace TopologyService
{
    public class TopologyConfig
    {
        public TopologyConfig()
        {
            Url = Environment.GetEnvironmentVariable("TopologyService.Url");
        }

        public string Url { get; }
    }
}
