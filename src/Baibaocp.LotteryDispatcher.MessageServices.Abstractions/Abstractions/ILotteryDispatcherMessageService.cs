using Baibaocp.LotteryDispatcher.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.MessageServices.Abstractions
{
    public interface ILotteryDispatcherMessageService<TExecuter> where TExecuter : IExecuter
    {
        Task PublishAsync(string merchanerId, TExecuter executer);

        Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken);
    }
}
