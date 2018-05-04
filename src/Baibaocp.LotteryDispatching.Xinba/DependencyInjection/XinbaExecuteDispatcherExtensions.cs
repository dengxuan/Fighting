using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using Baibaocp.LotteryDispatching.Xinba.Dispatchers;

namespace Baibaocp.LotteryDispatching.Xinba.DependencyInjection
{
    public static class XinbaExecuteDispatcherExtensions
    {
        public static LotteryDispatcherBuilder UseXinbaExecuteDispatcher(this LotteryDispatcherBuilder lotteryDispatcherBuilder, DispatcherConfiguration dispatcherConfiguration)
        {
            lotteryDispatcherBuilder.Services.AddSingleton<IQueryingDispatcher, TicketingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton<IOrderingDispatcher, OrderingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton<IQueryingDispatcher, AwardingExecuteDispatcher>();
            lotteryDispatcherBuilder.Services.AddSingleton(dispatcherConfiguration);
            return lotteryDispatcherBuilder;
        }
    }
}
