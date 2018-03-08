using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Baibaocp.Storaging.Entities;
using System;

namespace Baibaocp.LotteryDispatching.Suicai.Abstractions.Extensions
{
    public static class LotteryCodeExtensions
    {
        public static string ToSuicaicode(this string code, OrderingMessage entity)
        {
            string castcode = string.Empty;
            switch (entity.LvpOrder.LotteryPlayId)
            {
                case (int)PlayTypes.Ssq_Single:
                case (int)PlayTypes.Ssq_Multiple:
                    castcode = code.ToNewCode();
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + castcode.Replace(',', '|').Replace("*", "|-") + "|-;";
                    break;
                case (int)PlayTypes.Ssq_FixedUnset:
                    castcode = code.ToNewCode();
                    string[] blue = code.Split('*')[1].Split(',');
                    if (blue.Length == 1)
                    {
                        castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + castcode.Replace(",", "|").Replace("@", "|-").Replace("*", "|-") + "|-;";
                    }
                    else {
                        castcode = "1;" + "3003;" + entity.LvpOrder.InvestTimes + ";" + castcode.Replace(",", "|").Replace("@", "|-").Replace("*", "|-") + "|-;";
                    }
                    break;
                case (int)PlayTypes.Dlt_Single:
                case (int)PlayTypes.Dlt_Multiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(',', '|').Replace("*", "|-") + "|-;";
                    break;
                case (int)PlayTypes.Pls_FrontSingle:
                case (int)PlayTypes.Pls_FrontMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace('*', '|').Replace(",","") + "|;";
                    break;
                case (int)PlayTypes.Pls_AnyThreeSingle:
                case (int)PlayTypes.Pls_AnySixSingle:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(',', '|') + "|;";
                    break;
                case (int)PlayTypes.Plw_FrontSingle:
                case (int)PlayTypes.Plw_FrontMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace('*', '|').Replace(",", "") + "|;";
                    break;
                case (int)PlayTypes.Sfc_Single:
                case (int)PlayTypes.Sfc_Multiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|") + "|;";
                    break;
                case (int)PlayTypes.Rj_Multiple:
                case (int)PlayTypes.Rj_Single:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|").Replace("*","-") + "|;";
                    break;
                    //任选
                case (int)PlayTypes.Syxw_AnyTowSingle:
                case (int)PlayTypes.Syxw_AnyTownMultiple:
                case (int)PlayTypes.Syxw_AnyThreeSingle:
                case (int)PlayTypes.Syxw_AnyThreeMultiple:
                case (int)PlayTypes.Syxw_AnyFourSingle:
                case (int)PlayTypes.Syxw_AnyFourMultiple:
                case (int)PlayTypes.Syxw_AnyFiveSingle:
                case (int)PlayTypes.Syxw_AnyFiveMultiple:
                case (int)PlayTypes.Syxw_AnySixSingle:
                case (int)PlayTypes.Syxw_AnySixMultiple:
                case (int)PlayTypes.Syxw_AnySevenSingle:
                case (int)PlayTypes.Syxw_AnySevenMultiple:
                case (int)PlayTypes.Syxw_AnyEightSingle:
                case (int)PlayTypes.Syxw_AnyEightMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|") + "|;";
                    break;
                    //任选胆拖
                case (int)PlayTypes.Syxw_AnyTowFixedUnset:
                case (int)PlayTypes.Syxw_AnyThreeFixedUnset:
                case (int)PlayTypes.Syxw_AnyFourFixedUnset:
                case (int)PlayTypes.Syxw_AnyFiveFixedUnset:
                case (int)PlayTypes.Syxw_AnySixFixedUnset:
                case (int)PlayTypes.Syxw_AnySevenFixedUnset:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace("@", "|*").Replace(",", "|") + "|;";
                    break;
                    //直选
                case (int)PlayTypes.Syxw_FrontOneSingle:
                case (int)PlayTypes.Syxw_FrontOneMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|") + "|;";
                    break;
                case (int)PlayTypes.Syxw_FrontTowFixedPositionSingle:
                case (int)PlayTypes.Syxw_FrontTowFixedPositionMultiple:
                case (int)PlayTypes.Syxw_FrontThreeFixedPositionSingle:
                case (int)PlayTypes.Syxw_FrontThreeFixedPositionMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace("*", "|-").Replace(",", "|") + "|-;";
                    break;
                    //组选
                case (int)PlayTypes.Syxw_FrontTowAnyPositionSingle:
                case (int)PlayTypes.Syxw_FrontTowAnyPositionMultiple:
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionSingle:
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|") + "|;";
                    break;
                    //组选胆拖
                case (int)PlayTypes.Syxw_FrontTowAnyPositionFixedUnset:
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionFixedUnset:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace("@", "|*").Replace(",", "|") + "|;";
                    break;
                case (int)PlayTypes.Sd_FrontSingle:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace("*", "|") + "|;";
                    break;
                case (int)PlayTypes.Sd_FrontMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace("*","|-").Replace(",", "|") + "|-;";
                    break;
                case (int)PlayTypes.Sd_AnyThreeSingle:
                case (int)PlayTypes.Sd_AnySixSingle:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|") + "|;";
                    break;
                case (int)PlayTypes.Sd_AnyThreeMultiple:
                case (int)PlayTypes.Sd_AnySixMultiple:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|") + "|-;";
                    break;
                case (int)PlayTypes.Ks_ThreeDiffSingle:
                case (int)PlayTypes.Ks_ThreeSameSingle:
                case (int)PlayTypes.Ks_TowSameSingle:
                case (int)PlayTypes.Ks_TowSameAll:
                case (int)PlayTypes.Ks_SumValue:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "|") + "|;";
                    break;
                case (int)PlayTypes.Ks_ThreeSameAll:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + "111|222|333|444|555|666|;";
                    break;
                case (int)PlayTypes.Ks_TowDiffSingle:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + code.Replace(",", "") + "|;";
                    break;
                case (int)PlayTypes.Ks_ThreeSeriesAll:
                    castcode = "1;" + entity.LvpOrder.LotteryPlayId.ToPlay(entity.LvpOrder.InvestType) + ";" + entity.LvpOrder.InvestTimes + ";" + "123|234|345|456|;";
                    break;
            }
            return castcode;
        }

        public static string ToBaibaoNumber(this string code,LotteryTypes Lottery)
        {
            switch (Lottery)
            {
                case LotteryTypes.Ssq:
                case LotteryTypes.Sd:
                case LotteryTypes.CqSsc:
                case LotteryTypes.SdSyxw:
                case LotteryTypes.GxSyxw:
                case LotteryTypes.JxKs:
                    return code.Replace(";", ",").Replace("|","*");
                default:
                     throw new ArgumentException("LotteryType Not Support: {0}", Lottery.ToString());
            }
        }

        internal static string ToNewCode(this string code)
        {
            return code.Replace("01", "1").Replace("02", "2").Replace("03", "3").Replace("04", "4").Replace("05", "5").Replace("06", "6")
                   .Replace("07", "7").Replace("08", "8").Replace("09", "9");
        }
    }
}
