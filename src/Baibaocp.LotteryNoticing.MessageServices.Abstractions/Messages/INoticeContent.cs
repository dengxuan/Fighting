using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryNotifier.MessageServices.Messages
{
    public interface INoticeContent
    {
        string LvpOrderId { get; set; }

        string LvpMerchanerId { get; set; }
    }
}
