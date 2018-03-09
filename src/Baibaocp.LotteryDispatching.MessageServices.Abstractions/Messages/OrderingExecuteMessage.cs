using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public class OrderingExecuteMessage : ExecuteMessage
    {

        public LvpOrderedMessage LvpOrder { get; }

        public OrderingExecuteMessage(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrderedMessage) : base(ldpOrderId, ldpVenderId)
        {
        }
    }
}
