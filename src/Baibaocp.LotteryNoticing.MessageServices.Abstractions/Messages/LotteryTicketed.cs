using System;
using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryNotifier.MessageServices.Messages
{
    public class LotteryTicketed : INoticeContent
    {

        public string LvpOrderId { get; set; }

        public string LvpMerchanerId { get; set; }

        public string TicketedNumber { get; set; }

        public DateTime TicketedTime { get; set; }

        public string TicketedOdds { get; set; }

        public LotteryTicketingTypes TicketingType { get; set; }

        public override string ToString() => $"[LvpOrderId:{LvpOrderId}, LvpMerchanerId:{LvpMerchanerId}, TicketedNumber:{TicketedNumber}, TicketedTime:{TicketedTime}, TicketedOdds:{TicketedOdds}, TicketingType:{TicketingType}]";
    }
}
