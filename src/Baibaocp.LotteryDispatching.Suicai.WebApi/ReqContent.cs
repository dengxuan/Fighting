using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.WebApi
{
    public class ReqContent
    {
        public string apiCode { get; set; }

        public string partnerId { get; set; }

        public string messageId { get; set; }

        public string content { get; set; }

        public string hmac { get; set; }
    }
}
