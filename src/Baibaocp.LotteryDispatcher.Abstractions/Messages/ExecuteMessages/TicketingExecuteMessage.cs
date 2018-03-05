using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Collections.Generic;

namespace Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages
{
    /// <summary>
    /// 待出票订单消息
    /// </summary>
    public class TicketingExecuteMessage : ExecuteMessage
    {
        public TicketingExecuteMessage(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder) : base(ldpVenderId)
        {
            LdpOrderId = ldpOrderId;
            LvpOrder = lvpOrder;
            TicketContext = new Dictionary<string, object>();
        }

        public string LdpOrderId { get; }

        public LvpOrderedMessage LvpOrder { get; }

        public IDictionary<string, object> TicketContext { get; set; }
    }
}
