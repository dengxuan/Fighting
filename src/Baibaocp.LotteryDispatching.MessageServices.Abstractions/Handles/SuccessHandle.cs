using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{

    public sealed class SuccessHandle : IExecuteHandle
    {
        public string TicketNumber { get; }

        public string TicketOdds { get; }

        public SuccessHandle(string ticketNumber, string ticketOdds)
        {
            TicketNumber = ticketNumber;
            TicketOdds = ticketOdds;
        }

        public Task<bool> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
