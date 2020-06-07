using System;

namespace ServiceCommon
{
    public class TimeSource : ITimeSource
    {
        public DateTime GetNow() => DateTime.Now;

        public DateTime GetUtcNow() => DateTime.UtcNow;
    }
}
