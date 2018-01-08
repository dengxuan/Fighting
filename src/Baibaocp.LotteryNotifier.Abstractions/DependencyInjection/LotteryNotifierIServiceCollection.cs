using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Builder;
using Baibaocp.LotteryNotifier.Internal;
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

            AddNoticeServices(services);

            var builder = new LotteryNotifierBuilder(services);
            builderAction.Invoke(builder);
            builder.Build();

            return services;
        }

        internal static void AddNoticeServices(IServiceCollection services)
        {
            services.AddSingleton<INoticeDispatcher, NoticeDispatcher>();
            services.AddSingleton<INoticeHandlerFactory, NoticeHandlerFactory>();
        }
    }
}
