using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public class Handle
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public override string ToString()
        {
            return $"Handlle [ Code: {Code} Message: {Message} Data: {Data} ]";
        }
    }
}
