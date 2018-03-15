using Baibaocp.LotteryDispatching.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Baibaocp.LotteryDispatching.DependencyInjection.Builder
{
    public class LotteryDispatcherBuilder
    {
        public IServiceCollection Services { get; }

        internal LotteryDispatcherBuilder(IServiceCollection services)
        {
            Services = services;
        }

        internal void Build()
        {
            Services.AddSingleton<IHostedService, AwardingDispatcherService>();
            Services.AddSingleton<IHostedService, OrderingDispatcherService>();
            Services.AddSingleton<IHostedService, TicketingDispatcherService>();
        }
    }
}
