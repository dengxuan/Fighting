using Baibaocp.LotteryOrdering.MessageServices.Messages;

namespace Baibaocp.LotteryNotifier.MessageServices.Messages
{
    public class LotteryAwarded : INoticeContent
    {
        public string LvpOrderId { get; set; }

        public string LvpMerchanerId { get; set; }

        public int BonusAmount { get; set; }

        public int AftertaxBonusAmount { get; set; }

        public LotteryAwardingTypes AwardingType { get; set; }

        public override string ToString() => $"[LvpOrderId:{LvpOrderId}, LvpMerchanerId:{LvpMerchanerId}, BonusAmount:{BonusAmount}, AftertaxBonusAmount:{AftertaxBonusAmount}, AwardingType:{AwardingType}]";
    }
}
