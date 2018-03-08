using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatching.MessageServices.DependencyInjection
{
    public static class LotteryDispatchingMessageBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryDispatchingMessageService(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<OrderingMessage>, LotteryOrderingMessageService>();
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<AwardingMessage>, LotteryAwardingMessageService>();
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<TicketingMessage>, LotteryTicketingMessageService>();
            return messageServiceBuilder;
        }
    }
}
