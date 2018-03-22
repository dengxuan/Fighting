using System;
using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryNotifier.MessageServices.Messages
{
    public class LotteryTicketed: INoticeContent
    {
        public string LvpOrderId { get; set; }

        public string LvpMerchanerId { get; set; }

        public string TicketOdds { get; set; }

        public LotteryTicketingTypes TicketingType { get; set; }
    }
}
