using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCommon
{
    public abstract class BaseServiceRunner
    {
        protected void RunStatelessService<T>(string typeName) where T : StatelessService
        {
        }
    }
}
