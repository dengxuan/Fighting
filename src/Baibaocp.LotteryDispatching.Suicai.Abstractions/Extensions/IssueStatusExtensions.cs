using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching.Suicai.Abstractions.Extensions
{
    internal static class IssueStatusExtensions
    {
        internal static int ToBaiBaoStatus(this string issueResults)
        {
            switch (issueResults)
            {
                case "0":
                    return 10102;
                case "1":
                case "2":
                    return 10103;
                case "3":
                    return 10101;
                case "4":
                case "5":
                case "6":
                    return 10100;
                default: return 10199;
            }
        }
    }
}
