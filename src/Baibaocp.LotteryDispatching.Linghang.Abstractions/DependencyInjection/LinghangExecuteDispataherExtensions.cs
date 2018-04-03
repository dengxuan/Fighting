using System;
using Baibaocp.LotteryDispatching;
using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Baibaocp.LotteryDispatching.Linghang.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatching.Linghang.DependencyInjection
{
    public static class LinghangExecuteDispataherExtensions
    {
        public static LotteryDispatcherBuilder UseJinghangExecuteDispatcher(this LotteryDispatcherBuilder lotteryDispatcherBuilder, DispatcherConfiguration dispatcherConfiguration)
        {
            lotteryDispatcherBuilder.Services.AddSingleton<IOrderingDispatcher, OrderingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton<IQueryingDispatcher, QueryingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton(dispatcherConfiguration);
            return lotteryDispatcherBuilder;
        }
    }
}
