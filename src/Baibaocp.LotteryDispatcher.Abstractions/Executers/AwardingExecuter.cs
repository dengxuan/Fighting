using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryDispatcher.Executers
{
    /// <summary>
    /// 待返奖订单
    /// </summary>
    public class AwardingExecuter : Executer
    {
        public AwardingExecuter(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
        }

        public string LdpOrderId { get; }

        public LvpOrderedMessage LvpOrder { get; }
    }
}
