using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    public interface ILotteryTicketingMessageServiceManager
    {
        Task PublishAsync(LdpTicketedMessage ticketedMessage);
    }
}
