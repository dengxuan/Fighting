using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface ILotteryDispatcherMessageService<TMessage> where TMessage : IExecuteMessage
    {
        Task PublishAsync(string merchanerId, TMessage message);

        Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken);
    }
}
