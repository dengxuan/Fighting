using Baibaocp.LotteryAwardCalculator.Internal;
using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryAwardCalculator.Abstractions
{
    public interface ICalculator
    {
        Handle Calculate(LdpTicketedMessage ticketedMessage);
    }
}
