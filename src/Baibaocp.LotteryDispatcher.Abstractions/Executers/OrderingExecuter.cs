using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatching.Executers
{
    public class OrderingExecuter : Executer
    {
        public OrderingExecuter(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
        }

        public string LdpOrderId { get; }

        public LvpOrderedMessage LvpOrder { get; }
    }
}
