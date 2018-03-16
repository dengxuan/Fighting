using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryOrdering.MessageServices.Messages
{
    public class LvpTicketedMessage
    {
        public string OrderId { get; set; }

        public string VenderId { get; set; }

        public string TicketOdds { get; set; }

        public LotteryTicketingTypes TicketingType { get; set; }
    }
}
