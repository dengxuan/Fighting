using Baibaocp.Storaging.Entities;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Enrichers.Attributes;
using System.Collections.Generic;

namespace Baibaocp.LotteryOrdering.Messages
{
    public class TicketedMessage
    {

        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public LvpOrderMessage LvpOrder { get; set; }

        public string TicketOdds { get; set; }

        public OrderStatus Status { get; set; }
    }
}
