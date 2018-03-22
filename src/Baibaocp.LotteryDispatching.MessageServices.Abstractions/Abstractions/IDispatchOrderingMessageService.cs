using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IDispatchOrderingMessageService
    {
        Task PublishAsync(string merchanerId, string ldpOrderId, LvpOrderedMessage message);

        Task SubscribeAsync(string merchanerName, Func<OrderingExecuteMessage, Task<bool>> subscriber, CancellationToken stoppingToken);
    }
}
