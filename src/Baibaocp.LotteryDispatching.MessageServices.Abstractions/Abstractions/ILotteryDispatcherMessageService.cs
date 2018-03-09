using Baibaocp.LotteryDispatching.MessageServices.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface ILotteryDispatcherMessageService<in TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {
        Task PublishAsync(string merchanerId, TExecuteMessage message);

        Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken);
    }
}
