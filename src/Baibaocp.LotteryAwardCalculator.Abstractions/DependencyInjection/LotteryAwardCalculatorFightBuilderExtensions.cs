using Baibaocp.DependencyInjection.Builder;
using Fighting.DependencyInjection.Builder;
using System;

namespace Fighting.DependencyInjection
{
    public static class LotteryAwardCalculatorFightBuilderExtensions
    {
        public static FightBuilder ConfigureLotteryAwardCalculator(this FightBuilder fightBuilder, Action<LotteryAwardCalculatorBuilder> setupAction)
        {
            if (fightBuilder == null)
            {
                throw new ArgumentNullException(nameof(fightBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var builder = new LotteryAwardCalculatorBuilder(fightBuilder.Services);
            setupAction?.Invoke(builder);
            builder.Build();

            return fightBuilder;
        }
    }
}
