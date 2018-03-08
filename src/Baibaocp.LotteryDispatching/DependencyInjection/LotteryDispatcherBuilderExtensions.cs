using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Baibaocp.LotteryDispatching.MessageServices;
using Fighting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Baibaocp.LotteryDispatching.DependencyInjection
{
    public static class LotteryDispatcherBuilderExtensions
    {
        public static LotteryDispatcherBuilder UseLotteryDispatching<TExecuteMessage>(this LotteryDispatcherBuilder lotteryDispatcherBuilder, Action<DispatcherOptions> setupOptions) where TExecuteMessage : IExecuteMessage
        {
            lotteryDispatcherBuilder.Services.Configure(setupOptions);
            lotteryDispatcherBuilder.Services.AddSingleton<BackgroundService, DispatcherService<TExecuteMessage>>();
            return lotteryDispatcherBuilder;
        }
    }
}
