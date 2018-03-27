using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public class OrderingDispatchMessage : DispatchMessage
    {

        public LvpOrderMessage LvpOrder { get; }

        public OrderingDispatchMessage(string ldpOrderId, string ldpMerchanerId, LvpOrderMessage lvpOrder) : base(ldpOrderId, ldpMerchanerId)
        {
            LvpOrder = lvpOrder;
        }
    }
}
