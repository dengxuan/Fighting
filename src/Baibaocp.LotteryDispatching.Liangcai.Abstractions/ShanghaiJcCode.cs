using System;
using System.Collections.Generic;
using System.Linq;
using Baibaocp.Storaging.Entities.Extensions;

namespace Baibaocp.LotteryOrdering.Liangcai.Extensions
{
    public class ShanghaiJcCode
    {
        public static string ReturnShanghaiCode(string investCode, int lotteryId, int playId)
        {
            string liangcaicode = string.Empty;
            List<string> list = new List<string>();
            List<long> eventidlist = new List<long>();
            List<string> codelist = investCode.TrimEnd('^').Split('^').ToList();
            foreach (string code in codelist)
            {
                string[] eventarr = code.Split('|');
                string neweventid = eventarr[0].Substring(2) + eventarr[2];
                int lotid;
                string oldcode = string.Empty;
                string newcode = string.Empty;
                if (lotteryId == 20205)
                {
                    lotid = eventarr[3].ToBaibaoLottery();
                    oldcode = eventarr[4];
                    newcode = lotid.ToShanghaiCodeLottery() + ">" + neweventid + "=" + oldcode.ToShanghaiJcCode(lotid);
                }
                else if (lotteryId == 20405)
                {
                    lotid = eventarr[3].ToBaibaoLcLottery();
                    oldcode = eventarr[4];
                    newcode = lotid.ToShanghaiCodeLottery() + ">" + neweventid + "=" + oldcode.ToShanghaiJcCode(lotid);
                }
                else
                {
                    lotid = lotteryId;
                    oldcode = eventarr[3];
                    newcode = neweventid + "=" + oldcode.ToShanghaiJcCode(lotid);
                }
                eventidlist.Add(Convert.ToInt64(eventarr[0] + eventarr[1] + eventarr[2]));

                list.Add(newcode);
            }
            liangcaicode = lotteryId.ToShanghaiCodeLottery() + "|" + string.Join(",", list) + "|" + $"L{playId}".ToJingcaiLottery();
            return liangcaicode;
        }
    }
}
