using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching.Suicai.Ticketing
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
