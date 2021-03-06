﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.WebApi
{
    public class ResContent
    {
        public string version { get; set; }

        public string apiCode { get; set; }

        public string partnerId { get; set; }

        public string messageId { get; set; }

        public string content { get; set; }

        public string resCode { get; set; }

        public string resMsg { get; set; }

        public string hmac { get; set; }
    }
}
