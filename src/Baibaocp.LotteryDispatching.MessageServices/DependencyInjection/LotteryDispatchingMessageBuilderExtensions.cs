using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatching.MessageServices.DependencyInjection
{
    public static class LotteryDispatchingMessageBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryDispatchingMessageService(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<QueryingExecuteMessage>, LotteryDispatcherMessageService<QueryingExecuteMessage>>();
            messageServiceBuilder.Services.AddSingleton<ILotteryDispatcherMessageService<OrderingExecuteMessage>, LotteryDispatcherMessageService<OrderingExecuteMessage>>();
            return messageServiceBuilder;
        }
    }
}
