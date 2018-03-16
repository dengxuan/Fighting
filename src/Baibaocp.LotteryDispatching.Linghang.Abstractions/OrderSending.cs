using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching.Linghang.Abstractions
{
    public class OrderSending
    {
        public string issueNumber { get; set; }

        public int gameId { get; set; }

        public List<Tickets> tickets { get; set; }
    }

    public class Tickets
    {
        public int playType { get; set; }

        public string betType { get; set; }

        public string ticketSn { get; set; }

        public int multiple { get; set; }

        public int icount { get; set; }

        public int amount { get; set; }

        public string betContent { get; set; }
    }
}
