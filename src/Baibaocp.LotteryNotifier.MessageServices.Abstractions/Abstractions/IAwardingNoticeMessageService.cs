using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.MessageServices.Abstractions
{
    public interface IAwardingNoticeMessageService
    {
        Task PublishAsync(LvpAwardedMessage message);

        Task SubscribeAsync(Func<LvpAwardedMessage, Task<bool>> subscriber, CancellationToken stoppingToken);
    }
}
