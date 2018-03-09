using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Baibaocp.LotteryDispatching.DependencyInjection
{
    public static class LotteryDispatcherBuilderExtensions
    {
        public static LotteryDispatcherBuilder UseLotteryDispatching<TExecuteMessage>(this LotteryDispatcherBuilder lotteryDispatcherBuilder, Action<DispatcherOptions> setupOptions) where TExecuteMessage : IExecuteMessage
        {
            lotteryDispatcherBuilder.Services.Configure(setupOptions);
            lotteryDispatcherBuilder.Services.AddSingleton<IExecuteDispatcher<TExecuteMessage>, LotteryDispatcher<TExecuteMessage>>();
            lotteryDispatcherBuilder.Services.AddSingleton<IHostedService, DispatcherService<TExecuteMessage>>();
            return lotteryDispatcherBuilder;
        }
    }
}
