using Baibaocp.LotteryDispatching;
using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Baibaocp.LotteryDispatching.Suicai.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatcher.Liangcai.DependencyInjection
{
    public static class SuicaiExecuteDispatcherExtensions
    {
        public static LotteryDispatcherBuilder UseSuicaiExecuteDispatcher(this LotteryDispatcherBuilder lotteryDispatcherBuilder, DispatcherConfiguration dispatcherConfiguration)
        {
            lotteryDispatcherBuilder.Services.AddSingleton<IQueryingDispatcher, QueryingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton<IOrderingDispatcher, OrderingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton(dispatcherConfiguration);
            return lotteryDispatcherBuilder;
        }
    }
}
