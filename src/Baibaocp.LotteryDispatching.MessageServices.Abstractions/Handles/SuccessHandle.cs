using Baibaocp.LotteryDispatching.MessageServices.Abstractions;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{

    public sealed class SuccessHandle : IQueryingHandle
    {
        public string TicketNumber { get; }

        public string TicketOdds{ get; }

        public SuccessHandle(string ticketNumber, string ticketOdds)
        {
            TicketNumber = ticketNumber;
            TicketOdds = ticketOdds;
        }
    }
}
