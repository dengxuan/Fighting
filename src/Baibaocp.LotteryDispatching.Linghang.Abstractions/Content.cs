using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching.Linghang.Abstractions
{
    public class Content
    {
        public Head head { get; set; }

        public string body { get; set; }
    }

    public class Head
    {
        public string cmd { get; set; }

        public string userId { get; set; }

        public string timeStamp { get; set; }

        public string version { get; set; }

        public string sign { get; set; }

        public int status { get; set; }

        public int statusDes { get; set; }
    }
}
