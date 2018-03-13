using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching.Suicai.Abstractions
{
    public class ResContent
    {
        public string messageId { get; set; }

        public string apiCode { get; set; }

        public string content { get; set; }

        public string resCode { get; set; }

        public string resMsg { get; set; }

        public string hmac { get; set; }
    }
}
