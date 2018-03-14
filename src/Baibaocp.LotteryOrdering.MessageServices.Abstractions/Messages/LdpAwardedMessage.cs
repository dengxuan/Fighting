using Baibaocp.Storaging.Entities;

namespace Baibaocp.LotteryOrdering.MessageServices.Messages
{
    public class LdpAwardedMessage
    {

        public string LdpOrderId { get; set; }

        public string LdpVenderId { get; set; }

        public int BonusAmount { get; set; }

        public int AftertaxAmount { get; set; }

        public LdpAwardingTypes AwardingType { get; set; }
    }
}
