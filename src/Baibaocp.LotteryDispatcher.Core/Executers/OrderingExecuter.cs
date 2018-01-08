using Baibaocp.LotteryOrdering.Messages;

namespace Baibaocp.LotteryDispatcher.Core.Executers
{
    /// <summary>
    /// 待投注消息
    /// </summary>
    public class OrderingExecuter : Executer
    {
        public OrderingExecuter(string ldpOrderId, string ldpVenderId, LvpOrderMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
        }

        public string LdpOrderId { get; }

        public LvpOrderMessage LvpOrder { get; }
    }
}
