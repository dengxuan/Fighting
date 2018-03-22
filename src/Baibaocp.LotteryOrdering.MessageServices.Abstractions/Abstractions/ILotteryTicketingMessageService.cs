using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    public interface ILotteryTicketingMessageService: IMessageService
    {

        Task SubscribeAsync(CancellationToken stoppingToken);
    }
}
