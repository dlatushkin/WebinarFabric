using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCommon
{
    public interface IRemoteServices
    {
        ITopologyService TopologyService { get; }
    }
}
