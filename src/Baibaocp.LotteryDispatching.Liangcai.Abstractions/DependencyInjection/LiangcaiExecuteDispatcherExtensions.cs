using Baibaocp.LotteryDispatching;
using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Baibaocp.LotteryDispatching.Liangcai.Dispatchers;
using Baibaocp.LotteryDispatching.Liangcai.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Baibaocp.LotteryDispatcher.Liangcai.DependencyInjection
{
    public static class LiangcaiExecuteDispatcherExtensions
    {
        public static LotteryDispatcherBuilder UseLiangcaiExecuteDispatcher(this LotteryDispatcherBuilder lotteryDispatcherBuilder, DispatcherConfiguration dispatcherConfiguration)
        {
            lotteryDispatcherBuilder.Services.AddSingleton<IAwardingExecuteDispatcher, AwardingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton<IOrderingExecuteDispatcher, OrderingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton<ITicketingExecuteDispatcher, TicketingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton(dispatcherConfiguration);
            return lotteryDispatcherBuilder;
        }
    }
}
