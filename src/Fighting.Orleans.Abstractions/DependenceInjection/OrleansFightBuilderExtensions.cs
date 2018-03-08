using Fighting.DependencyInjection.Builder;
using Fighting.Orleans.DependencyInjection.Builder;
using System;

namespace Fighting.Orleans.DependenceInjection
{
    public static class OrleansFightBuilderExtensions
    {
        public static FightBuilder ConfigureOrleans(this FightBuilder fightBuilder, Action<OrleansBuilder> setupAction)
        {
            OrleansBuilder builder = new OrleansBuilder(fightBuilder.Services);
            setupAction?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
