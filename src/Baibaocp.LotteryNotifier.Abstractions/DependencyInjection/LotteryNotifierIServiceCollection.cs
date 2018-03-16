using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Builder;
using Baibaocp.LotteryNotifier.Internal.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            AddNoticeServices(services);

            var builder = new LotteryNotifierBuilder(services);
            builderAction.Invoke(builder);
            builder.Build();

            return services;
        }

        internal static void AddNoticeServices(IServiceCollection services)
        {
            services.AddSingleton<ITicketingNotifier, TicketingNotifier>();
            services.AddSingleton<IAwardingNotifier, AwardingNotifier>();
            services.AddSingleton<IHostedService, LotteryAwardedService>();
            services.AddSingleton<IHostedService, LotteryTicketedService>();
        }
    }
}
