using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    public interface ILotteryTicketingMessageService
    {
        Task PublishAsync(LdpTicketedMessage ticketedMessage);
    }
}
