using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryNotifier.MessageServices.Messages
{
    public class LotteryAwarded: INoticeContent
    {
        public string LvpOrderId { get; set; }

        public string LvpMerchanerId { get; set; }

        public int BonusAmount { get; set; }

        public int AftertaxBonusAmount { get; set; }

        public LotteryAwardingTypes AwatdingType { get; set; }
    }
}
