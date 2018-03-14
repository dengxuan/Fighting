using Baibaocp.LotteryDispatching.MessageServices.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IQueryingMessageService
    {

        Task PublishAsync(string merchanerId, string ldpOrderId, QueryingTypes queryingType);

        Task SubscribeAsync(string merchanerId, string merchanerName, QueryingTypes queryingType, Func<QueryingExecuteMessage, Task<bool>> subscriber, CancellationToken stoppingToken);
    }
}
