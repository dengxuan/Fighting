using Baibaocp.Storaging.Entities;
using System;

namespace Baibaocp.LotteryDispatching.Suicai.Abstractions.Extensions
{
    public static class LotteryExtensions
    {
        public static string ToSuicaiLottery(this int lotteryType)
        {
            switch (lotteryType)
            {
                case (int)LotteryTypes.Ssq: return "4";
                case (int)LotteryTypes.Sd: return "6";
                case (int)LotteryTypes.Dlt:return "2001";
                case (int)LotteryTypes.Pls: return "2003";
                case (int)LotteryTypes.Plw: return "2004";
                case (int)LotteryTypes.ZcSfc:
                case (int)LotteryTypes.ZcR9: return "2006";
                case (int)LotteryTypes.JxKs: return "19";
                case (int)LotteryTypes.CqSsc: return "404";
                case (int)LotteryTypes.SdSyxw: return "2005";
                case (int)LotteryTypes.GxSyxw: return "2007";
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }

        public static string ToPlay(this int playType, bool type)
        {
            switch (playType)
            {
                case (int)PlayTypes.Ssq_Single: return "3000";
                case (int)PlayTypes.Ssq_Multiple: return "3001";
                case (int)PlayTypes.Ssq_FixedUnset: return "3002";
                case (int)PlayTypes.Dlt_Single:
                    if (type == true)
                    {
                        return "11";
                    }
                    else
                    {
                        return "1";
                    }
                case (int)PlayTypes.Dlt_Multiple:return "4";
                case (int)PlayTypes.Dlt_FixedUnset:return "7";
                case (int)PlayTypes.Pls_FrontSingle:return "1";
                case (int)PlayTypes.Pls_FrontMultiple:return "2";
                case (int)PlayTypes.Pls_AnyThreeSingle:return "8";
                case (int)PlayTypes.Pls_AnyThreeMultiple:return "6";
                case (int)PlayTypes.Pls_AnySixSingle:return "4";
                case (int)PlayTypes.Pls_AnySixMultiple: return "5";
                case (int)PlayTypes.Plw_FrontSingle:return "1";
                case (int)PlayTypes.Plw_FrontMultiple:return "2";
                case (int)PlayTypes.Ks_ThreeDiffSingle: 
                case (int)PlayTypes.Ks_TowSameSingle:
                case (int)PlayTypes.Ks_ThreeSameSingle: return "9300";
                case (int)PlayTypes.Ks_SumValue: return "9330";
                case (int)PlayTypes.Ks_TowSameAll: return "9320";
                case (int)PlayTypes.Ks_ThreeSameAll: return "9310";
                case (int)PlayTypes.Ks_TowDiffSingle: return "9340";
                case (int)PlayTypes.Ks_ThreeSeriesAll: return "9350";
                case (int)PlayTypes.Sfc_Single:
                case (int)PlayTypes.Sfc_Multiple: return "6001";
                case (int)PlayTypes.Rj_Single:
                case (int)PlayTypes.Rj_Multiple: return "6002";
                case (int)PlayTypes.Syxw_AnyTowSingle:
                case (int)PlayTypes.Syxw_AnyTownMultiple: return "24001";
                case (int)PlayTypes.Syxw_AnyTowFixedUnset: return "24002";
                case (int)PlayTypes.Syxw_AnyThreeSingle:
                case (int)PlayTypes.Syxw_AnyThreeMultiple: return "24003";
                case (int)PlayTypes.Syxw_AnyThreeFixedUnset: return "24004";
                case (int)PlayTypes.Syxw_AnyFourSingle:
                case (int)PlayTypes.Syxw_AnyFourMultiple: return "24005";
                case (int)PlayTypes.Syxw_AnyFourFixedUnset: return "24006";
                case (int)PlayTypes.Syxw_AnyFiveSingle:
                case (int)PlayTypes.Syxw_AnyFiveMultiple: return "24007";
                case (int)PlayTypes.Syxw_AnyFiveFixedUnset: return "24008";
                case (int)PlayTypes.Syxw_AnySixSingle:
                case (int)PlayTypes.Syxw_AnySixMultiple: return "24009";
                case (int)PlayTypes.Syxw_AnySixFixedUnset: return "24010";
                case (int)PlayTypes.Syxw_AnySevenSingle:
                case (int)PlayTypes.Syxw_AnySevenMultiple: return "24011";
                case (int)PlayTypes.Syxw_AnySevenFixedUnset: return "24012";
                case (int)PlayTypes.Syxw_AnyEightSingle:
                case (int)PlayTypes.Syxw_AnyEightMultiple: return "24013";
                case (int)PlayTypes.Syxw_FrontOneSingle:
                case (int)PlayTypes.Syxw_FrontOneMultiple: return "24014";
                case (int)PlayTypes.Syxw_FrontTowFixedPositionSingle:
                case (int)PlayTypes.Syxw_FrontTowFixedPositionMultiple: return "24015";
                case (int)PlayTypes.Syxw_FrontTowAnyPositionSingle:
                case (int)PlayTypes.Syxw_FrontTowAnyPositionMultiple: return "24016";
                case (int)PlayTypes.Syxw_FrontTowAnyPositionFixedUnset: return "24017";
                case (int)PlayTypes.Syxw_FrontThreeFixedPositionSingle:
                case (int)PlayTypes.Syxw_FrontThreeFixedPositionMultiple: return "24018";
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionSingle:
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionMultiple: return "24019";
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionFixedUnset: return "24020";
                case (int)PlayTypes.Sd_FrontSingle: return "2000";
                case (int)PlayTypes.Sd_FrontMultiple: return "2020";
                case (int)PlayTypes.Sd_AnyThreeSingle: return "2010";
                case (int)PlayTypes.Sd_AnySixSingle: return "2011";
                case (int)PlayTypes.Sd_AnyThreeMultiple: return "2030";
                case (int)PlayTypes.Sd_AnySixMultiple: return "2031";
                default: throw new ArgumentException("PlayType Not Support: {0}", playType.ToString());
            }
        }

        public static int ToIssueNumber(this string issueNumber, int lotteryType)
        {
            switch (lotteryType)
            {
                case (int)LotteryTypes.Ssq:
                case (int)LotteryTypes.Sd:
                case (int)LotteryTypes.CqSsc:
                case (int)LotteryTypes.JxKs:
                case (int)LotteryTypes.SdSyxw:
                case (int)LotteryTypes.GxSyxw: return Convert.ToInt32(issueNumber);
                case (int)LotteryTypes.Plw:
                case (int)LotteryTypes.Pls:
                case (int)LotteryTypes.Dlt:
                case (int)LotteryTypes.ZcSfc:
                case (int)LotteryTypes.ZcR9: return Convert.ToInt32("20" + issueNumber);
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }

        public static string FromIssueNumber(this int? issueNumber, int lotteryType)
        {
            if (!issueNumber.HasValue)
                return "";
            switch (lotteryType)
            {
                case (int)LotteryTypes.Ssq:
                case (int)LotteryTypes.Sd:
                case (int)LotteryTypes.CqSsc:
                case (int)LotteryTypes.JxKs:
                case (int)LotteryTypes.SdSyxw:
                case (int)LotteryTypes.GxSyxw: return issueNumber.ToString();
                case (int)LotteryTypes.Pls:
                case (int)LotteryTypes.Plw:
                case (int)LotteryTypes.Dlt:
                case (int)LotteryTypes.ZcSfc:
                case (int)LotteryTypes.ZcR9: return issueNumber.ToString().Substring(2);
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }
    }
}
