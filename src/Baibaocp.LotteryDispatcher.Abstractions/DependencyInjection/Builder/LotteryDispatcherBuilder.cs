using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

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
        }
    }
}
