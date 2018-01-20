using Baibaocp.Storaging.Entities;
using Baibaocp.LotteryNotifier.Abstractions;

namespace Baibaocp.LotteryNotifier.Notifiers
{
    public class Ticketed
    {
        public string OrderId { get; set; }

        public string TicketOdds { get; set; }

        public int Status { get; set; }
    }
}
