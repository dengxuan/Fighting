using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatching.MessageServices.DependencyInjection
{
    public static class LotteryDispatcherMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryDispatchingMessagePublisher(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<IDispatchOrderingMessageService, DispatchOrderingMessagePublisher>();
            messageServiceBuilder.Services.AddSingleton<IDispatchQueryingMessageService, DispatchQueryingMessagePublisher>();
            return messageServiceBuilder;
        }
    }
}
