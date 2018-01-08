using Baibaocp.LotteryOrdering.Messages;

namespace Baibaocp.LotteryDispatcher.Core.Executers
{
    /// <summary>
    /// 待返奖订单
    /// </summary>
    public class AwardingExecuter : Executer
    {
        public AwardingExecuter(string ldpOrderId, string ldpVenderId, LvpOrderMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
        }

        public string LdpOrderId { get; }

        public LvpOrderMessage LvpOrder { get; }
    }
}
