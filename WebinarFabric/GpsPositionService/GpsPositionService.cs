using System.Fabric;
using ServiceCommon;
using ServiceInterfaces;

namespace GpsPositionService
{
    public class GpsPositionService : BaseStatefulService, IGpsPositionService
    {
        public GpsPositionService(StatefulServiceContext context)
            : base(context)
        { }
    }
}
