using Baibaocp.LotteryAwardCalculator;
using Microsoft.Extensions.Options;

namespace Baibaocp.DependencyInjection.Builder
{
    internal class LotteryAwardCalculatorOptionsSetup : ConfigureOptions<LotteryAwardCalculatorOptions>
    {
        public LotteryAwardCalculatorOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(LotteryAwardCalculatorOptions options)
        {
        }
    }
}
