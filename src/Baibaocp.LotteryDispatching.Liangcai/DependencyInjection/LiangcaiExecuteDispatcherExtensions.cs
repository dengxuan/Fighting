using Baibaocp.LotteryDispatching;
using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Baibaocp.LotteryDispatching.Liangcai.Dispatchers;
using Baibaocp.LotteryDispatching.Liangcai.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatcher.Liangcai.DependencyInjection
{
    public static class LiangcaiExecuteDispatcherExtensions
    {
        public static LotteryDispatcherBuilder UseLiangcaiExecuteDispatcher(this LotteryDispatcherBuilder lotteryDispatcherBuilder, DispatcherConfiguration dispatcherConfiguration)
        {
            lotteryDispatcherBuilder.Services.AddSingleton<IOrderingDispatcher, OrderingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton<IQueryingDispatcher, QueryingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton(dispatcherConfiguration);
            return lotteryDispatcherBuilder;
        }
    }
}
