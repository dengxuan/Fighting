using Fighting.DependencyInjection.Builder;
using Fighting.Scheduling.DependencyInjection.Builder;
using System;

namespace Fighting.Scheduling.DependencyInjection
{
    public static class SchedulingFightBuilderExtensions
    {
        public static FightBuilder ConfigureScheduling(this FightBuilder fightBuilder, Action<SchedulingBuilder> setupAction)
        {
            var builder = new SchedulingBuilder(fightBuilder.Services, fightBuilder);
            setupAction?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
