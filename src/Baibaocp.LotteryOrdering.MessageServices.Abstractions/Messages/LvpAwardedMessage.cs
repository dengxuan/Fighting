using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryOrdering.MessageServices.Messages
{
    public class LvpAwardedMessage
    {

        public string OrderId { get; set; }

        public string VenderId { get; set; }

        public int BonusAmount { get; set; }

        public int AftertaxAmount { get; set; }

        public LotteryAwardingTypes AwardingType { get; set; }
    }
}
