using Baibaocp.Storaging.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Xinba.Extensions
{
    internal static class LotteryCodeExtensions
    {
        internal static string ToCastcode(this string code, int playType, int lottery)
        {
            string castcode = string.Empty;
            switch (lottery) {
                case (int)LotteryTypes.Dlt:
                    switch (playType)
                    {
                        case (int)PlayTypes.Dlt_Single:
                            castcode = code.Replace(",", "").Replace('*', '|') + "^";
                            break;
                        case (int)PlayTypes.Dlt_Multiple:
                            string[] mcodearr = code.Split('*');
                            castcode = "*" + mcodearr[0].Replace(",", "") + "|*" + mcodearr[1].Replace(",", "") + "^";
                            break;
                        case (int)PlayTypes.Dlt_FixedUnset:
                            string[] codearr = code.Split('*');
                            string[] qiancode = codearr[0].Split('@');
                            string[] houcode = codearr[1].Split('@');
                            string qian = qiancode.Length > 1 ? qiancode[0] + "*" + qiancode[1] : "*" + qiancode[0];
                            string hou = houcode.Length > 1 ? houcode[0] + "*" + houcode[1] : "*" + houcode[0];
                            castcode = qian.Replace(",", "") + "|" + hou.Replace(",", "");
                            break;
                    }
                    break;
                case (int)LotteryTypes.Pls:
                    switch (playType)
                    {
                        case (int)PlayTypes.Pls_FrontSingle:
                            castcode = code + "^";
                            break;
                        case (int)PlayTypes.Pls_FrontMultiple:
                            castcode = code.Replace(",", "") + "^";
                            break;
                        case (int)PlayTypes.Pls_AnyThreeSingle:
                        case (int)PlayTypes.Pls_AnySixSingle:
                            castcode = code.Replace(",", "*") + "^";
                            break;
                        case (int)PlayTypes.Pls_FrontSum:
                        case (int)PlayTypes.Pls_AnySixMultiple:
                        case (int)PlayTypes.Pls_AnyThreeMultiple:
                        case (int)PlayTypes.Pls_AnySixSum:
                        case (int)PlayTypes.Pls_AnyThreeSum:
                            castcode = "**" + code.Replace(",", "") + "^";
                            break;
                    }
                    break;
                case (int)LotteryTypes.Plw:
                    switch (playType)
                    {
                        case (int)PlayTypes.Plw_FrontSingle:
                            castcode = code + "^";
                            break;
                        case (int)PlayTypes.Plw_FrontMultiple:
                            castcode = code.Replace(",", "") + "^";
                            break;
                    }
                    break;
                case (int)LotteryTypes.Qxc:
                    switch (playType)
                    {
                        case (int)PlayTypes.Qxc_Single:
                            castcode = code + "^";
                            break;
                        case (int)PlayTypes.Qxc_Multiple:
                            castcode = code.Replace(",", "") + "^";
                            break;
                    }
                    break;
                case (int)LotteryTypes.JcHun:
                    castcode = code.Replace("20201", "FT001").Replace("20202", "FT002").Replace("20203", "FT003").Replace("20204", "FT004").Replace("20206", "FT006");
                    break;
                case (int)LotteryTypes.LcHun:
                    castcode = code.Replace("20401", "BSK001").Replace("20402", "BSK002").Replace("20403", "BSK003").Replace("20404", "BSK004");
                    break;
                default:
                    castcode = code;
                    break;
            }
            return castcode;
        }

        internal static string ToXinbaJcCode(string code, int lottery)
        {
            string xinbacode = string.Empty;
            switch (lottery)
            {
                case (int)LotteryTypes.JcHun:
                    xinbacode = code.Replace("20201", "FT001").Replace("20202", "FT002").Replace("20203", "FT003").Replace("20204", "FT004").Replace("20206", "FT006");
                    break;
                case (int)LotteryTypes.LcHun:
                    xinbacode = code.Replace("20401", "BSK001").Replace("20402", "BSK002").Replace("20403", "BSK003").Replace("20404", "BSK004");
                    break;
                default:
                    xinbacode = code;
                    break;
            }
            return xinbacode;
        }

        internal static string ToBaibaoCode(this string code)
        {
            return code.Replace('$', '@').Replace('|', '*');
        }
    }
}
