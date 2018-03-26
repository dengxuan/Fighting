using Baibaocp.LotteryDispatching.MessageServices.Messages;

namespace Baibaocp.LotteryOrdering.Scheduling
{
    public class AwardingScheduleArgs
    {
        public long LdpOrderId { get; set; }

        public string LdpMerchanerId { get; set; }

        public string LvpOrderId { get; set; }

        public string LvpMerchanerId { get; set; }

        public int LotteryId { get; set; }
    }
}
