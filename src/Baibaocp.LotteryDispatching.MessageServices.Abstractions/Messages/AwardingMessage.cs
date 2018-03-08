using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching
{
    /// <summary>
    /// 待返奖订单
    /// </summary>
    public class AwardingMessage : ExecuteMessage
    {
        public AwardingMessage(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
        }

        public string LdpOrderId { get; }

        public LvpOrderedMessage LvpOrder { get; }
    }
}
