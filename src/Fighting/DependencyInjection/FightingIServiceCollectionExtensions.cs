using Fighting.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fighting.DependencyInjection
{
    public static class FightingIServiceCollectionExtensions
    {
        public static IServiceCollection AddFighting(this IServiceCollection services, Action<FightBuilder> setupAction)
        {
            FightBuilder builder = new FightBuilder(services);
            setupAction?.Invoke(builder);
            builder.Build();
            return services;
        }
    }
}
