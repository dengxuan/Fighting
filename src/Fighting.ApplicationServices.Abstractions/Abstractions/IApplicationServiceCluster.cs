using System;
using System.Collections.Generic;
using System.Text;

namespace Fighting.ApplicationServices.Abstractions
{
    public interface IApplicationServiceCluster
    {
        TApplicationService GetApplicationService<TApplicationService>() where TApplicationService : IApplicationService;
    }
}
