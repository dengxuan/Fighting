using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    public interface ILotteryAwardingMessageService
    {
        Task PublishAsync(LdpAwardedMessage message);

        Task SubscribeAsync(CancellationToken stoppingToken);
    }
}
