﻿using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Fighting.DependencyInjection.Builder;
using System;

namespace Baibaocp.LotteryDispatching.DependencyInjection
{
    public static class LotteryDispatcherFightBuilderExtensions
    {
        public static FightBuilder ConfigureLotteryDispatcher(this FightBuilder fightBuilder, Action<LotteryDispatcherBuilder> setupAction)
        {
            if (fightBuilder == null)
            {
                throw new ArgumentNullException(nameof(fightBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var builder = new LotteryDispatcherBuilder(fightBuilder.Services);
            setupAction?.Invoke(builder);
            builder.Build();

            return fightBuilder;
        }
    }
}
