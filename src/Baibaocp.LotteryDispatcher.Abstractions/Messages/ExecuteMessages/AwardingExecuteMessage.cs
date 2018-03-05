using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages
{
    /// <summary>
    /// 待返奖订单
    /// </summary>
    public class AwardingExecuteMessage : ExecuteMessage
    {
        public AwardingExecuteMessage(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
        }

        public string LdpOrderId { get; }

        public LvpOrderedMessage LvpOrder { get; }
    }
}
