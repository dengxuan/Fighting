using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Internal.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace Baibaocp.LotteryNotifier.Builder
{
    public class LotteryNotifierBuilder
    {

        public IServiceCollection Services { get; }

        internal LotteryNotifierBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public LotteryNotifierBuilder ConfigureOptions(Action<LotteryNoticeOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<LotteryNoticeOptions>, DefaultLotteryNoticeOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<LotteryNoticeOptions>>().Value);

            Services.AddSingleton<ITicketingNotifier, TicketingNotifier>();
            Services.AddSingleton<IAwardingNotifier, AwardingNotifier>();
            Services.AddSingleton<INoticeSerializer, JsonNoticeSerializer>();

            Services.AddSingleton<IHostedService, LotteryAwardedSubscriber>();
            Services.AddSingleton<IHostedService, LotteryTicketedSubscriber>();
        }
    }
}
