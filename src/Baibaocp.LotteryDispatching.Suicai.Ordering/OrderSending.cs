using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching.Suicai.Ordering
{
    public class OrderSending
    {
        public string gameId { get; set; }

        public string issue { get; set; }

        public List<order> orderList { get; set; }
    }
    public class order
    {
        public string orderId { get; set; }

        public string timeStamp { get; set; }

        public string ticketMoney { get; set; }

        public string betCount { get; set; }

        public string betDetail { get; set; }
    }
}
