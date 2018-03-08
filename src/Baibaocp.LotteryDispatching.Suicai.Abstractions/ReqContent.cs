using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching.Suicai.Abstractions
{
    public class ReqContent
    {
        public string version { get; set; }

        public string apiCode { get; set; }

        public string partnerId { get; set; }

        public string messageId { get; set; }

        public string content { get; set; }

        public string hmac { get; set; }
    }
}
