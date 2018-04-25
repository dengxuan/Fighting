using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryOrdering.Scheduling
{
    public class OrderingScheduleArgs
    {
        public string LdpOrderId { get; set; }

        public string LdpMerchanerId { get; set; }

        public LvpOrderMessage Message { get; set; }
    }
}
