using Microsoft.Extensions.Options;

namespace Baibaocp.LotteryNotifier.Builder
{
    internal class DefaultLotteryNoticeOptionsSetup : ConfigureOptions<LotteryNoticeOptions>
    {
        public DefaultLotteryNoticeOptionsSetup()
            : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(LotteryNoticeOptions options)
        {
        }
    }
}
