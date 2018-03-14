using System.Collections.Generic;

namespace Baibaocp.LotteryDispatching.Suicai
{
    public class OrderTicket
    {
        public List<Ticket> orderList { get; set; }
    }
    public class Ticket
    {
        public string orderId { get; set; }

    }
}
