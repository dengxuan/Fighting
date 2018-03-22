using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public class OrderingExecuteMessage : ExecuteMessage
    {

        public LvpOrderMessage LvpOrder { get; }

        public OrderingExecuteMessage(long ldpOrderId, string ldpMerchanerId, LvpOrderMessage lvpOrder) : base(ldpOrderId, ldpMerchanerId)
        {
            LvpOrder = lvpOrder;
        }
    }
}
