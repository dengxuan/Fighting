using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryNotifier.MessageServices.DependencyInjection
{
    public static class LotteryNotifierMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryNoticingMessagePublisher(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddTransient<ILotteryNoticingMessagePublisher, LotteryNoticingMessageService>();
            return messageServiceBuilder;
        }
    }
}
