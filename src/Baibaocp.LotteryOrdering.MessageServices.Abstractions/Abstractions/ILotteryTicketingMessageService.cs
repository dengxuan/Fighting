using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    public interface ILotteryTicketingMessageService: IMessageService
    {
        Task PublishAsync(LdpTicketedMessage message);

        Task SubscribeAsync(CancellationToken stoppingToken);
    }
}
