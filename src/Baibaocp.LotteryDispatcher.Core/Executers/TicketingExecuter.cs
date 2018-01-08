using Baibaocp.LotteryOrdering.Messages;
using System.Collections.Generic;

namespace Baibaocp.LotteryDispatcher.Core.Executers
{
    /// <summary>
    /// 待出票订单
    /// </summary>
    public class TicketingExecuter : Executer
    {
        public TicketingExecuter(string ldpOrderId, string ldpVenderId, LvpOrderMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
            TicketContext = new Dictionary<string, object>();
        }

        public string LdpOrderId { get; }

        public LvpOrderMessage LvpOrder { get; }

        public IDictionary<string, object> TicketContext { get; set; }
    }
}
