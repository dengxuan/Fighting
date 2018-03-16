using Baibaocp.LotteryNotifier.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryNotifier.Builder
{
    public class LotteryNotifierBuilder
    {
        private readonly DiscoverySettings _discoverySettings = new DiscoverySettings();

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

        public LotteryNotifierBuilder AddHandlerDiscovery(Action<DiscoverySettings> discoverySettings)
        {
            discoverySettings(_discoverySettings);
            return this;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<LotteryNoticeOptions>, DefaultLotteryNoticeOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<LotteryNoticeOptions>>().Value);
        }
    }
}
