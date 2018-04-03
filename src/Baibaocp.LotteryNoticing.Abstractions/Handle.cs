using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public class Handle
    {
        public int Ret { get; set; }

        public string Msg { get; set; }

        public string Data { get; set; }

        public override string ToString()
        {
            return $"Handlle [ Ret=>{Ret} Msg=>{Msg} Data=>{Data} ]";
        }
    }
}
