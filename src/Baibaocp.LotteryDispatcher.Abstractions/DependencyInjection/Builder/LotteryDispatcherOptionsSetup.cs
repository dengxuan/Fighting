using Microsoft.Extensions.Options;

namespace Baibaocp.LotteryDispatcher.DependencyInjection.Builder
{
    internal class LotteryDispatcherOptionsSetup : ConfigureOptions<LotteryDispatcherOptions>
    {
        public LotteryDispatcherOptionsSetup()
            : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(LotteryDispatcherOptions options)
        {
        }
    }
}
