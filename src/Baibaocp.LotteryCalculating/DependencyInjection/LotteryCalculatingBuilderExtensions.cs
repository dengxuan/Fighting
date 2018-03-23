using Baibaocp.LotteryCalculating.Abstractions;
using Baibaocp.LotteryCalculating.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryCalculating.DependencyInjection
{
    public static class LotteryCalculatingBuilderExtensions
    {
        public static LotteryCalculatingBuilder UseLotteryCalculating(this LotteryCalculatingBuilder lotteryCalculatingBuilder)
        {
            lotteryCalculatingBuilder.Services.AddSingleton<ILotteryCalculatorFactory, LotteryCalculatorFactory>();
            return lotteryCalculatingBuilder;
        }
    }
}
