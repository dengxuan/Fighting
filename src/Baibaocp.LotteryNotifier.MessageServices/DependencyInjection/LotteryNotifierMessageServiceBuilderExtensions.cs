using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryNotifier.MessageServices.DependencyInjection
{
    public static class LotteryNotifierMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryNotifierMessageService(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<IAwardingNoticeMessageService, AwardingNoticeMessageService>();
            messageServiceBuilder.Services.AddSingleton<ITicketingNoticeMessageService, TicketingNoticeMessageService>();
            return messageServiceBuilder;
        }
    }
}
