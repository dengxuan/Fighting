using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface ILotteryDispatcherMessageSubscriber
    {
        Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken);
    }
}
