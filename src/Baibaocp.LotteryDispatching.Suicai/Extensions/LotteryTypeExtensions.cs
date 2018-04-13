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
                case (int)LotteryTypes.GxKs: return "9";
                case (int)LotteryTypes.CqSsc: return "404";
                case (int)LotteryTypes.SdSyxw: return "2005";
                case (int)LotteryTypes.GxSyxw: return "2007";
                case (int)LotteryTypes.HbSyxw: return "2008";
                default: throw new ArgumentException("LotteryType Not Support: {0}", lotteryType.ToString());
            }
        }

        public static string ToPlay(this int playType,int lottery, bool type)
        {
            string play = string.Empty;
            switch (lottery)
            {
                case (int)LotteryTypes.Dlt:
                    switch (playType)
                    {
                        case (int)PlayTypes.Dlt_Single:
                            if (type == true)
                            {
                                play = "11";
                            }
                            else
                            {
                                play = "1";
                            }
                            break;
                        case (int)PlayTypes.Dlt_Multiple: play = "4"; break;
                        case (int)PlayTypes.Dlt_FixedUnset: play = "7"; break;
                    }
                    break;
                case (int)LotteryTypes.Ssq:
                    switch (playType)
                    {
                        case (int)PlayTypes.Ssq_Single: play = "3000"; break;
                        case (int)PlayTypes.Ssq_Multiple: play = "3001"; break;
                        case (int)PlayTypes.Ssq_FixedUnset: play = "3002"; break;
                    }
                    break;
                case (int)LotteryTypes.Pls:
                    switch (playType)
                    {
                        case (int)PlayTypes.Pls_FrontSingle: play = "1"; break;
                        case (int)PlayTypes.Pls_FrontMultiple: play = "2"; break;
                        case (int)PlayTypes.Pls_AnyThreeSingle: play = "8"; break;
                        case (int)PlayTypes.Pls_AnyThreeMultiple: play = "6"; break;
                        case (int)PlayTypes.Pls_AnySixSingle: play = "4"; break;
                        case (int)PlayTypes.Pls_AnySixMultiple: play = "5"; break;
                    }
                    break;
                case (int)LotteryTypes.Plw:
                    switch (playType)
                    {
                        case (int)PlayTypes.Plw_FrontSingle: play = "1"; break;
                        case (int)PlayTypes.Plw_FrontMultiple: play = "2"; break;
                    }
                    break;
                case (int)LotteryTypes.GxKs:
                    switch (playType)
                    {
                        case (int)PlayTypes.Ks_ThreeDiffSingle: play = "9330"; break;
                        case (int)PlayTypes.Ks_TowSameSingle: play = "9321"; break;
                        case (int)PlayTypes.Ks_ThreeSameSingle: play = "9311"; break;
                        case (int)PlayTypes.Ks_SumValue: play = "9300"; break;
                        case (int)PlayTypes.Ks_TowSameAll: play = "9320"; break;
                        case (int)PlayTypes.Ks_ThreeSameAll: play = "9310"; break;
                        case (int)PlayTypes.Ks_TowDiffSingle: play = "9340"; break;
                        case (int)PlayTypes.Ks_ThreeSeriesAll: play = "9350"; break;
                    }
                    break;
                case (int)LotteryTypes.JxKs:
                    switch (playType)
                    {
                        case (int)PlayTypes.Ks_ThreeDiffSingle:
                        case (int)PlayTypes.Ks_TowSameSingle:
                        case (int)PlayTypes.Ks_ThreeSameSingle: play = "9300"; break;
                        case (int)PlayTypes.Ks_SumValue: play = "9330"; break;
                        case (int)PlayTypes.Ks_TowSameAll: play = "9320"; break;
                        case (int)PlayTypes.Ks_ThreeSameAll: play = "9310"; break;
                        case (int)PlayTypes.Ks_TowDiffSingle: play = "9340"; break;
                        case (int)PlayTypes.Ks_ThreeSeriesAll: play = "9350"; break;
                    }
                    break;
                case (int)LotteryTypes.ZcSfc:
                    switch (playType)
                    {
                        case (int)PlayTypes.Sfc_Single:
                        case (int)PlayTypes.Sfc_Multiple: play = "6001"; break;
                    }
                    break;
                case (int)LotteryTypes.ZcR9:
                    switch (playType)
                    {
                        case (int)PlayTypes.Rj_Single:
                        case (int)PlayTypes.Rj_Multiple: play = "6002"; break;
                    }
                    break;
                case (int)LotteryTypes.GxSyxw:
                    switch (playType)
                    {
                        case (int)PlayTypes.Syxw_AnyTowSingle:
                        case (int)PlayTypes.Syxw_AnyTownMultiple: play = "24001"; break;
                        case (int)PlayTypes.Syxw_AnyTowFixedUnset: play = "24002"; break;
                        case (int)PlayTypes.Syxw_AnyThreeSingle:
                        case (int)PlayTypes.Syxw_AnyThreeMultiple: play = "24003"; break;
                        case (int)PlayTypes.Syxw_AnyThreeFixedUnset: play = "24004"; break;
                        case (int)PlayTypes.Syxw_AnyFourSingle:
                        case (int)PlayTypes.Syxw_AnyFourMultiple: play = "24005"; break;
                        case (int)PlayTypes.Syxw_AnyFourFixedUnset: play = "24006"; break;
                        case (int)PlayTypes.Syxw_AnyFiveSingle:
                        case (int)PlayTypes.Syxw_AnyFiveMultiple: play = "24007"; break;
                        case (int)PlayTypes.Syxw_AnyFiveFixedUnset: play = "24008"; break;
                        case (int)PlayTypes.Syxw_AnySixSingle:
                        case (int)PlayTypes.Syxw_AnySixMultiple: play = "24009"; break;
                        case (int)PlayTypes.Syxw_AnySixFixedUnset: play = "24010"; break;
                        case (int)PlayTypes.Syxw_AnySevenSingle:
                        case (int)PlayTypes.Syxw_AnySevenMultiple: play = "24011"; break;
                        case (int)PlayTypes.Syxw_AnySevenFixedUnset: play = "24012"; break;
                        case (int)PlayTypes.Syxw_AnyEightSingle:
                        case (int)PlayTypes.Syxw_AnyEightMultiple: play = "24013"; break;
                        case (int)PlayTypes.Syxw_FrontOneSingle:
                        case (int)PlayTypes.Syxw_FrontOneMultiple: play = "24014"; break;
                        case (int)PlayTypes.Syxw_FrontTowFixedPositionSingle:
                        case (int)PlayTypes.Syxw_FrontTowFixedPositionMultiple: play = "24015"; break;
                        case (int)PlayTypes.Syxw_FrontTowAnyPositionSingle:
                        case (int)PlayTypes.Syxw_FrontTowAnyPositionMultiple: play = "24016"; break;
                        case (int)PlayTypes.Syxw_FrontTowAnyPositionFixedUnset: play = "24017"; break;
                        case (int)PlayTypes.Syxw_FrontThreeFixedPositionSingle:
                        case (int)PlayTypes.Syxw_FrontThreeFixedPositionMultiple: play = "24018"; break;
                        case (int)PlayTypes.Syxw_FrontThreeAnyPositionSingle:
                        case (int)PlayTypes.Syxw_FrontThreeAnyPositionMultiple: play = "24019"; break;
                        case (int)PlayTypes.Syxw_FrontThreeAnyPositionFixedUnset: play = "24020"; break;
                    }
                    break;
                case (int)LotteryTypes.HbSyxw:
                    switch (playType)
                    {
                        case (int)PlayTypes.Syxw_AnyTowSingle:
                        case (int)PlayTypes.Syxw_AnyTownMultiple: play = "23001"; break;
                        case (int)PlayTypes.Syxw_AnyTowFixedUnset: play = "23002"; break;
                        case (int)PlayTypes.Syxw_AnyThreeSingle:
                        case (int)PlayTypes.Syxw_AnyThreeMultiple: play = "23003"; break;
                        case (int)PlayTypes.Syxw_AnyThreeFixedUnset: play = "23004"; break;
                        case (int)PlayTypes.Syxw_AnyFourSingle:
                        case (int)PlayTypes.Syxw_AnyFourMultiple: play = "23005"; break;
                        case (int)PlayTypes.Syxw_AnyFourFixedUnset: play = "23006"; break;
                        case (int)PlayTypes.Syxw_AnyFiveSingle:
                        case (int)PlayTypes.Syxw_AnyFiveMultiple: play = "23007"; break;
                        case (int)PlayTypes.Syxw_AnyFiveFixedUnset: play = "23008"; break;
                        case (int)PlayTypes.Syxw_AnySixSingle:
                        case (int)PlayTypes.Syxw_AnySixMultiple: play = "23009"; break;
                        case (int)PlayTypes.Syxw_AnySixFixedUnset: play = "23010"; break;
                        case (int)PlayTypes.Syxw_AnySevenSingle:
                        case (int)PlayTypes.Syxw_AnySevenMultiple: play = "23011"; break;
                        case (int)PlayTypes.Syxw_AnySevenFixedUnset: play = "23012"; break;
                        case (int)PlayTypes.Syxw_AnyEightSingle:
                        case (int)PlayTypes.Syxw_AnyEightMultiple: play = "23013"; break;
                        case (int)PlayTypes.Syxw_FrontOneSingle:
                        case (int)PlayTypes.Syxw_FrontOneMultiple: play = "23014"; break;
                        case (int)PlayTypes.Syxw_FrontTowFixedPositionSingle:
                        case (int)PlayTypes.Syxw_FrontTowFixedPositionMultiple: play = "23015"; break;
                        case (int)PlayTypes.Syxw_FrontTowAnyPositionSingle:
                        case (int)PlayTypes.Syxw_FrontTowAnyPositionMultiple: play = "23016"; break;
                        case (int)PlayTypes.Syxw_FrontTowAnyPositionFixedUnset: play = "23017"; break;
                        case (int)PlayTypes.Syxw_FrontThreeFixedPositionSingle:
                        case (int)PlayTypes.Syxw_FrontThreeFixedPositionMultiple: play = "23018"; break;
                        case (int)PlayTypes.Syxw_FrontThreeAnyPositionSingle:
                        case (int)PlayTypes.Syxw_FrontThreeAnyPositionMultiple: play = "23019"; break;
                        case (int)PlayTypes.Syxw_FrontThreeAnyPositionFixedUnset: play = "23020"; break;
                    }
                    break;
                case (int)LotteryTypes.Sd:
                    switch (playType)
                    {
                        case (int)PlayTypes.Sd_FrontSingle: play = "2000"; break;
                        case (int)PlayTypes.Sd_FrontMultiple: play = "2020"; break;
                        case (int)PlayTypes.Sd_AnyThreeSingle: play = "2010"; break;
                        case (int)PlayTypes.Sd_AnySixSingle: play = "2011"; break;
                        case (int)PlayTypes.Sd_AnyThreeMultiple: play = "2030"; break;
                        case (int)PlayTypes.Sd_AnySixMultiple: play = "2031"; break;
                    }
                    break;
                default: throw new ArgumentException("玩法{0}不支持", playType.ToString());
            }
            return play;
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
                case (int)LotteryTypes.HbSyxw:
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
                case (int)LotteryTypes.GxKs:
                case (int)LotteryTypes.SdSyxw:
                case (int)LotteryTypes.HbSyxw:
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
