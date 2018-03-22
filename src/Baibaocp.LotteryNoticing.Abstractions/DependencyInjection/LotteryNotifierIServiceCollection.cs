using Baibaocp.LotteryNotifier.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Baibaocp.LotteryNotifier.DependencyInjection
{
    public static class LotteryNotifierIServiceCollection
    {
        public static IServiceCollection AddLotteryNotifier(this IServiceCollection services, Action<LotteryNotifierBuilder> builderAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (builderAction == null)
            {
                throw new ArgumentNullException(nameof(builderAction));
            }

            var builder = new LotteryNotifierBuilder(services);
            builderAction.Invoke(builder);
            builder.Build();

            return services;
        }
    }
}
