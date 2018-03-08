using Baibaocp.LotteryDispatching.Executers;
using Baibaocp.LotteryDispatching.MessageServices;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatching.DependencyInjection
{
    public static class LotteryDispatchingMessageBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryDispatchingMessageService(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<OrderingExecuter>, LotteryOrderingMessageService>();
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<AwardingExecuter>, LotteryAwardingMessageService>();
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<TicketingExecuter>, LotteryTicketingMessageService>();
            return messageServiceBuilder;
        }
    }
}
