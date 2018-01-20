using System;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Extensions
{
    public static class LotteryCodeExtensions
    {
        public static string ToCastcode(this string code, int playType)
        {
            string castcode = string.Empty;
            switch (playType)
            {
                case 10021071:
                case 10022071:
                    castcode = code.Replace(',',' ').Replace('*', '-');
                    break;
                case 10023071:
                    castcode = code.Replace(',',' ').Replace('@', '$').Replace('*', '-');
                    break;
                case 10051071:
                    castcode = code.Replace('*', ',');
                    break;
                case 10052071:
                    castcode = code.Replace(",", "").Replace('*', ',');
                    break;
                case 10041031:
                case 10042031:
                    castcode = playType.ToPlay() + "|" + code.Replace(",", "").Replace("*", ",");
                    break;
                case 10041032:
                case 10041062:
                    castcode = playType.ToPlay() + "|" + code;
                    break;
                case 10051051:
                case 10052051:
                    castcode = code.Replace(",", "").Replace("*", ",");
                    break;

                
            }
            
            return castcode;
        }


        public static string ToShanghaiJcCode(this string code, int lottery)
        {
            string result = string.Empty;
            switch (lottery)
            {
                case 20201:
                case 20401:
                case 20402:
                    string[] spflist = code.ParseOneBettCode();
                    result = string.Join("/", spflist.ToArray());
                    break;

                case 20206:
                    string[] rqspflist = code.ParseOneBettCode();
                    result = string.Join("/", rqspflist.ToArray());
                    break;

                case 20202:
                    string[] bflist = code.ToJcCode(":");
                    result = string.Join("/", bflist);
                    break;

                case 20204:
                    string[] bqclist = code.ToJcCode("-");
                    result = string.Join("/", bqclist);
                    break;

                case 20203:
                    string[] zjqlist = code.ParseOneBettCode();               
                    result = string.Join("/", zjqlist.ToArray());
                    break;

                case 20404:
                    code = code.Replace("1", "3").Replace("2", "0");
                    string[] dxflist = code.ParseOneBettCode();
                    result = string.Join("/", dxflist.ToArray());
                    break;
                case 20403:
                    string[] sfclist = code.ParseTwoBettCode();
                    result = string.Join("/", sfclist.ToArray());
                    break;
            }
            return result;
        }


        /// <summary>
        /// 转换号码(每一位)
        /// </summary>
        /// <param name="Code">号码</param>
        /// <returns></returns>
        public static string[] ParseOneBettCode(this string Code)
        {
            Int32 length = Code.Length;
            Int32 size = length;
            string[] retval = new string[size];
            for (int i = 0; i < size; i++)
            {
                retval[i] = Code.Substring(i * 1, 1);
            }
            return retval;
        }

        /// <summary>
        /// 转换号码(每两位)
        /// </summary>
        /// <param name="Code">号码</param>
        /// <returns></returns>
        public static string[] ParseTwoBettCode(this string Code)
        {
            Int32 length = Code.Length;
            Int32 size = length / 2;
            string[] retval = new string[size];
            for (int i = 0; i < size; i++)
            {
                retval[i] = Code.Substring(i * 2, 2);
            }
            return retval;
        }

        public static string[] ToJcCode(this string Code,string a)
        {
            Int32 length = Code.Length;
            Int32 size = length / 2;
            string[] retval = new string[size];
            for (int i = 0; i < size; i++)
            {
                retval[i] =  Code.Substring(i * 2, 2).Insert(1, a);
            }
            return retval;
        }
    }
}
