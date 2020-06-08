using ClusterModels.Lines;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TopologyService.Logics;
using Xunit;

namespace WebinarTests
{
    public class TopologyTests
    {
        [Fact]
        public void Test1()
        {
            var lineStations = new Dictionary<Line, Station[]>
            {
                [new Line { Id = "L01", Name = "1st Line" }] = new[] { new Station { Code = "OSL", Name = "Oslo" } }
            };

            var s = JsonConvert.SerializeObject(lineStations);
        }

        [Fact]
        public void Test2()
        {
            (Line Line, Station[] Stations)[] topology = new[]
            {
                (
                    Line: new Line { Id = "L01" },
                    Stations: new Station[0] 
                ),
                (
                    Line: new Line { Id = "L02" },
                    Stations: new Station[0]
                )
            };

            var s = JsonConvert.SerializeObject(topology);
        }

        [Fact]
        public void Test3()
        {
            var json = File.ReadAllText(@"Data\Topology.json");

            var topology = JsonConvert.DeserializeObject<TopologyLogic.TopologyRecord[]>(json);
        }
    }
}
