using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching
{
    public class OrderingMessage : ExecuteMessage
    {
        public OrderingMessage(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
        }

        public string LdpOrderId { get; }

        public LvpOrderedMessage LvpOrder { get; }
    }
}
