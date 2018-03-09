using Microsoft.Extensions.Options;

namespace Baibaocp.LotteryDispatching.DependencyInjection.Builder
{
    internal class LotteryDispatcherOptionsSetup : ConfigureOptions<DispatcherOptions>
    {
        public LotteryDispatcherOptionsSetup()
            : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(DispatcherOptions options)
        {
        }
    }
}
