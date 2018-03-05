using Baibaocp.Storaging.Entities;

namespace Baibaocp.LotteryOrdering.MessageServices.Messages
{
    public class LdpTicketedMessage
    {

        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public LvpOrderedMessage LvpOrder { get; set; }

        public string TicketOdds { get; set; }

        public OrderStatus Status { get; set; }
    }
}
