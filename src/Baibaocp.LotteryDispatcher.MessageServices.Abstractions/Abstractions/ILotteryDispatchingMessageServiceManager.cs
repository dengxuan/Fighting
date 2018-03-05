using Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    /// <summary>
    /// 分发消息到投注接口
    /// </summary>
    public interface ILotteryDispatchingMessageServiceManager
    {
        Task PublishAsync(string merchanerId, OrderingExecuteMessage lvpOrderedMessage);

        Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken);
    }
}
