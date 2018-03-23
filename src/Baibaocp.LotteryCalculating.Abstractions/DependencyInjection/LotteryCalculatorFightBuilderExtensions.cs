using Baibaocp.LotteryCalculating.DependencyInjection.Builder;
using Fighting.DependencyInjection.Builder;
using System;

namespace Baibaocp.LotteryCalculating.DependencyInjection
{
    public static class LotteryCalculatorFightBuilderExtensions
    {
        public static FightBuilder ConfigureLotteryCalculating(this FightBuilder fightBuilder, Action<LotteryCalculatingBuilder> lotteryCaclulatingBuilder)
        {
            var builder = new LotteryCalculatingBuilder(fightBuilder.Services);
            lotteryCaclulatingBuilder?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
