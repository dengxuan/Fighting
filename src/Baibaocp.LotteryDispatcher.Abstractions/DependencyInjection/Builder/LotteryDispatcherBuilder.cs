using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Baibaocp.LotteryDispatching.DependencyInjection.Builder
{
    public class LotteryDispatcherBuilder
    {
        public IServiceCollection Services { get; }

        internal LotteryDispatcherBuilder(IServiceCollection services)
        {
            Services = services;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<DispatcherOptions>, LotteryDispatcherOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<DispatcherOptions>>().Value);
            Services.AddSingleton<IExecuteDispatcher<OrderingExecuteMessage>, LotteryDispatcher<OrderingExecuteMessage>>();
            Services.AddSingleton<IExecuteDispatcher<QueryingExecuteMessage>, LotteryDispatcher<QueryingExecuteMessage>>();
        }
    }
}
