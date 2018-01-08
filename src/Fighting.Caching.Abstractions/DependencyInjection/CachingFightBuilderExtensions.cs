using Fighting.DependencyInjection.Builder;
using System;

namespace Fighting.DependencyInjection
{
    public static class CachingFightBuilderExtensions
    {
        public static FightBuilder ConfigureCacheing(this FightBuilder fightBuilder, Action<CachingBuilder> setupAction)
        {
            var builder = new CachingBuilder(fightBuilder.Services);
            setupAction?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
