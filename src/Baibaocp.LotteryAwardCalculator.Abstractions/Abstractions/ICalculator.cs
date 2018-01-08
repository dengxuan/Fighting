using Baibaocp.LotteryAwardCalculator.Internal;
using Baibaocp.LotteryOrdering.Messages;

namespace Baibaocp.LotteryAwardCalculator.Abstractions
{
    public interface ICalculator
    {
        Handle Calculate(TicketedMessage ticketedMessage);
    }
}
