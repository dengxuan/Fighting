using Baibaocp.Storaging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Xinba.Extensions
{
    internal static class LotteryExtensions
    {
        internal static string ToXinbaLottery(this int lotteryType)
        {
            switch (lotteryType)
            {
                case (int)LotteryTypes.JcSpf: return "FT001";
                case (int)LotteryTypes.JcRqspf: return "FT006";
                case (int)LotteryTypes.JcBf: return "FT002";
                case (int)LotteryTypes.JcBqc: return "FT004";
                case (int)LotteryTypes.JcZjq: return "FT003";
                case (int)LotteryTypes.JcHun: return "FT005";
                case (int)LotteryTypes.LcSf: return "BSK001";
                case (int)LotteryTypes.LcRfsf: return "BSK002";
                case (int)LotteryTypes.LcSfc: return "BSK003";
                case (int)LotteryTypes.LcDxf: return "BSK004";
                case (int)LotteryTypes.LcHun: return "BSK005";
                case (int)LotteryTypes.JcZc: return "1";
                case (int)LotteryTypes.JcLc: return "0";
                case (int)LotteryTypes.Dlt: return "T001";
                case (int)LotteryTypes.Qxc: return "D7";
                case (int)LotteryTypes.ZcSfc: return "D14";
                case (int)LotteryTypes.Pls: return "D3";
                case (int)LotteryTypes.Plw: return "D5";
                case (int)LotteryTypes.GxSyxw: return "GXC511";
                case (int)LotteryTypes.GdSyxw:return "GDC511";
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }

        internal static int ToXinbaPlay(this int Play)
        {
            switch(Play)
            {
                case (int)PlayTypes.Dlt_Single: return 0;
                case (int)PlayTypes.Dlt_Multiple: return 1;
                case (int)PlayTypes.Dlt_FixedUnset: return 2;
                case (int)PlayTypes.Pls_FrontSingle: return 0;
                case (int)PlayTypes.Pls_FrontMultiple: return 1;
                case (int)PlayTypes.Pls_FrontSum: return 2;
                case (int)PlayTypes.Pls_AnySixSingle:
                case (int)PlayTypes.Pls_AnyThreeSingle: return 3;
                case (int)PlayTypes.Pls_AnySixMultiple: return 4;
                case (int)PlayTypes.Pls_AnyThreeMultiple: return 5;
                case (int)PlayTypes.Pls_AnyThreeSum:
                case (int)PlayTypes.Pls_AnySixSum: return 6;
                case (int)PlayTypes.Plw_FrontSingle: return 0;
                case (int)PlayTypes.Plw_FrontMultiple: return 1;
                case (int)PlayTypes.Qxc_Single: return 0;
                case (int)PlayTypes.Qxc_Multiple: return 1;
                default: return (int)Play;
            }
        }

        internal static int ToBaibaoLottery(this string lotteryType)
        {
            switch (lotteryType)
            {
                case "FT001":return (int)LotteryTypes.JcSpf;
                case "FT006":return (int)LotteryTypes.JcRqspf;
                case "FT002": return (int)LotteryTypes.JcBf;
                case "FT003": return (int)LotteryTypes.JcZjq ;
                case "FT004": return (int)LotteryTypes.JcBqc;
                case "FT005": return (int)LotteryTypes.JcHun;
                case "BSK001": return (int)LotteryTypes.LcSf;
                case "BSK002": return (int)LotteryTypes.LcRfsf;
                case "BSK003": return (int)LotteryTypes.LcSfc;
                case "BSK004": return (int)LotteryTypes.LcDxf;
                case "BSK005": return (int)LotteryTypes.LcHun;
                case "T001": return (int)LotteryTypes.Dlt;
                case "D3": return (int)LotteryTypes.Pls;
                case "D5": return (int)LotteryTypes.Plw;
                case "D7": return (int)LotteryTypes.Qxc;
                case "GDC511":return (int)LotteryTypes.GdSyxw;
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }

        internal static int ToIssueNumber(this string issueNumber, int lotteryType)
        {
            switch (lotteryType)
            {
                case (int)LotteryTypes.Dlt:
                case (int)LotteryTypes.Pls:
                case (int)LotteryTypes.Plw:
                case (int)LotteryTypes.Qxc: return Convert.ToInt32(string.Format("20{1}", DateTime.Now, issueNumber));
                case (int)LotteryTypes.GdSyxw:return Convert.ToInt32(issueNumber);
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }

        internal static string FromIssueNumber(this int? issueNumber, int lotteryType)
        {
            switch (lotteryType)
            {
                case (int)LotteryTypes.Dlt:
                case (int)LotteryTypes.Pls:
                case (int)LotteryTypes.Plw:
                case (int)LotteryTypes.Qxc: return issueNumber.ToString().Substring(2);
                case (int)LotteryTypes.LcDxf:
                case (int)LotteryTypes.LcHun:
                case (int)LotteryTypes.LcRfsf:
                case (int)LotteryTypes.LcSf:
                case (int)LotteryTypes.LcSfc:
                case (int)LotteryTypes.JcBf:
                case (int)LotteryTypes.JcBqc:
                case (int)LotteryTypes.JcHun:
                case (int)LotteryTypes.JcRqspf:
                case (int)LotteryTypes.JcSpf:
                case (int)LotteryTypes.JcZjq: return "0";
                case (int)LotteryTypes.GdSyxw:return issueNumber.ToString();
                default: return "";
            }
        }
    }
}
