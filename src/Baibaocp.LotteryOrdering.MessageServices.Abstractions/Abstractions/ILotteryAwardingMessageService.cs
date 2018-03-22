using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    public interface ILotteryAwardingMessageService
    {

        Task SubscribeAsync(CancellationToken stoppingToken);
    }
}
