using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCommon
{
    public interface ITimeSource
    {
        DateTime GetNow();

        DateTime GetUtcNow();
    }
}
