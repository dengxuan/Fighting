using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Fighting.ApplicationServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Baibaocp.LotteryOrdering.ApplicationServices.DependencyInjection
{
    public static class LotteryOrderingApplicationServiceBuilderExtensions
    {
        public static ApplicationServiceBuilder UseLotteryOrderingApplicationService(this ApplicationServiceBuilder applicationServiceBuilder)
        {
            applicationServiceBuilder.Services.AddSingleton<IOrderingApplicationService, OrderingApplicationService>();
            return applicationServiceBuilder;
        }
    }
}
