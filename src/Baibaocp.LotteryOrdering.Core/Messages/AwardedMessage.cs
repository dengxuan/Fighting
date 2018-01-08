using Baibaocp.Core;
using RawRabbit.Configuration.Exchange;
using RawRabbit.Enrichers.Attributes;
using System.Collections.Generic;

namespace Baibaocp.LotteryOrdering.Messages
{
    public class AwardedMessage
    {

        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public LvpOrderMessage LvpOrder { get; set; }

        public int BonusAmount { get; set; }

        public int AftertaxAmount { get; set; }

        public OrderStatus Status { get; set; }
    }
}
