using Baibaocp.Storaging.Entities;

namespace Baibaocp.LotteryOrdering.MessageServices.Messages
{
    public class LdpAwardedMessage
    {

        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public LvpOrderedMessage LvpOrder { get; set; }

        public int BonusAmount { get; set; }

        public int AftertaxAmount { get; set; }

        public OrderStatus Status { get; set; }
    }
}
