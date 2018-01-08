using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Baibaocp.Core.Extensions;

namespace Baibaocp.LotteryOrdering.Validations
{
    public class SportsCodeValidator
    {
        protected string ValidChars { get; private set; }

        protected string[] zcspf = new string[] { "3", "1", "0" };

        protected string[] zcbf = new string[] { "10", "20", "21", "30", "31", "32", "40", "41", "42", "50", "51", "52", "01", "02", "12", "03", "13", "23", "04", "14", "24", "05", "15", "25", "00", "11", "22", "33", "09", "99", "90" };

        protected string[] zcbqc = new string[] { "00", "01", "03", "11", "10", "13", "33", "31", "30" };

        protected string[] zczjq = new string[] { "0", "1", "2", "3", "4", "5", "6", "7"};

        protected string[] lcsf = new string[] { "3","0"};

        protected string[] lcsfc = new string[] { "01", "02", "03", "04", "05", "06", "11", "12", "13", "14", "15", "16"};

        protected string[] lcdxf = new string[] { "1", "2"};
        public void Initialize()
        {
            this.ValidChars = string.Format("{0}{1}{2}{3}{4}{5}", Constants.SPLIT_CODE, Constants.SPLIT_FIXED, Constants.SPLIT_POSITION, Constants.SPLIT_STYLOR, Constants.PLACEHOLDER, Constants.SPLIT_COMPETITIVE);
        }

        public virtual bool Verify(dynamic content)
        {
            /* 判断投注倍数是否正确 */
            if (content.TimesCount < Constants.MIN_TIMES || content.TimesCount > Constants.MAX_TIMES)
            {
                return false;
            }
            int  num = int.Parse($"N{content.PlayType}".ToJingcaiLottery());
            string[] codelist = content.InvestCode.Split('^');
            List<string> eventlist = new List<string>();
            List<string> lotidlist = new List<string>();
            //投注赛事数量是否与串关相符
            if (num != 1)
            {
                if (num != codelist.Count() - 1)
                {
                    return false;
                }
            }
            else {
                if (codelist.Count() - 1 > 8)
                {
                    return false;
                }
            }
            List<string> _n_arr = new List<string>();
            List<string> _c_arr = new List<string>();
            _c_arr = content.PlayType.ToString().ToJingcaiLottery().Split(',').ToList();
            for (Int32 j = 0; j < codelist.Count()-1; j++)
            {
                string lotid = string.Empty;
                string[] playlist = codelist[j].Split('|');
                if (content.LotteryType == LotteryTypes.JcHun || content.LotteryType == LotteryTypes.LcHun)
                {
                    lotid = playlist[3];
                    if (!lotidlist.Contains(lotid))
                    {
                        lotidlist.Add(lotid);
                    }
                }
                else {
                    lotid = Convert.ToInt32(content.LotteryType).ToString();
                    if (playlist.Count() > 4)
                    {
                        return false;
                    }
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcSpf).ToString() || lotid == Convert.ToInt32(LotteryTypes.JcRqspf).ToString() ||
                    lotid == Convert.ToInt32(LotteryTypes.LcSf).ToString() || lotid == Convert.ToInt32(LotteryTypes.LcRfsf).ToString() || lotid == Convert.ToInt32(LotteryTypes.LcDxf).ToString())
                {
                    if (codelist.Count() - 1 > 8)
                    {
                        return LpOrderResults.InvestCodeFormatError;
                    }
                    _n_arr.Add(string.Join(",", this.ParseOneBettCode(playlist[playlist.Count() - 1])));
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcBf).ToString() || lotid == Convert.ToInt32(LotteryTypes.JcBqc).ToString() ||
                    lotid == Convert.ToInt32(LotteryTypes.LcSfc).ToString())
                {
                    if (num != 1)
                    {
                        if (codelist.Count() - 1 > 4)
                        {
                            return LpOrderResults.InvestCodeFormatError;
                        }
                    }
                    _n_arr.Add(string.Join(",", this.ParseTwoBettCode(playlist[playlist.Count() - 1])));
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcZjq).ToString())
                {
                    if (num != 1)
                    {
                        if (codelist.Count() - 1 > 6)
                        {
                            return LpOrderResults.InvestCodeFormatError;
                        }
                    }
                    _n_arr.Add(string.Join(",", this.ParseOneBettCode(playlist[playlist.Count() - 1])));
                }
                string eventid = playlist[0] + playlist[1] +playlist[2];
                eventlist.Add(eventid);
                //赛事是否可投
                if (!this.CheckEvent(int.Parse(lotid), content.PlayType, eventid))
                {
                    return LpOrderResults.NotSupportPlayType;
                }
                //投注码是否正确
                if (!this.CheckCode(lotid, playlist[playlist.Count()-1]))
                {
                    return LpOrderResults.InvestCodeFormatError;
                }
            }
            List<string> e =  eventlist.Distinct().ToList();
            if (e.Count != eventlist.Count)
            {
                return LpOrderResults.InvestCodeFormatError;
            }
            if (num == 1)
            {
                result = LpOrderResults.Succeed;
            }
            else if (e.Count != num)
            {
                result = LpOrderResults.InvestCodeFormatError;
            }
            if (content.LotteryType == LotteryTypes.JcHun || content.LotteryType == LotteryTypes.LcHun)
            {
                if (lotidlist.Count < 2)
                {
                    return LpOrderResults.InvestCodeFormatError;
                }
            }
            if (result == LpOrderResults.Succeed)
            {
                int count = this.clacZyBetSum(_n_arr, _c_arr);
                content.InvestCount = count;
                if (content.Amount != count * Constants.UNIT_PRICE * content.TimesCount)
                {
                    return LpOrderResults.InvestAmountError;
                }
            }
            return result;
        }

        /// <summary>
        /// 验证赛事是否可投注
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        protected bool CheckEvent(int lotid,PlayTypes playtype, string eventid)
        {
            bool flag = true;
            if (lotid > 20200 && lotid < 20207)
            {
                flag = this.CalcuStorer.GetXinbaEvent("20200", lotid, playtype, long.Parse(eventid));
            }
            if (lotid > 20400 && lotid < 20406)
            {
                flag = this.CalcuStorer.GetXinbaEvent("20400", lotid, playtype, long.Parse(eventid));
            }
            return flag;
        }

        protected bool CheckCode(string LotType, string code)
        {
            bool flag = true;
            string[] codelist;
            switch (LotType)
            {
                case "20201":
                case "20206":
                    codelist = this.ParseOneBettCode(code);
                    for (Int32 i = 0; i < codelist.Count(); i++)
                    {
                        if (!this.zcspf.Contains(codelist[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                case "20202":
                    codelist = this.ParseTwoBettCode(code);
                    for (Int32 i = 0; i < codelist.Count(); i++)
                    {
                        if (!this.zcbf.Contains(codelist[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                case "20204":
                    codelist = this.ParseTwoBettCode(code);
                    for (Int32 i = 0; i < codelist.Count(); i++)
                    {
                        if (!this.zcbqc.Contains(codelist[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                case "20203":
                    codelist = this.ParseOneBettCode(code);
                    for (Int32 i = 0; i < codelist.Count(); i++)
                    {
                        if (!this.zczjq.Contains(codelist[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                case "20402":
                case "20401":
                    codelist = this.ParseOneBettCode(code);
                    for (Int32 i = 0; i < codelist.Count(); i++)
                    {
                        if (!this.lcsf.Contains(codelist[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                case "20403":
                    codelist = this.ParseTwoBettCode(code);
                    for (Int32 i = 0; i < codelist.Count(); i++)
                    {
                        if (!this.lcsfc.Contains(codelist[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
                case "20404":
                    codelist = this.ParseOneBettCode(code);
                    for (Int32 i = 0; i < codelist.Count(); i++)
                    {
                        if (!this.lcdxf.Contains(codelist[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag;
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }


        /// <summary>
        /// 计算自由过关注数
        /// </summary>
        /// <param name="_n_arr">选择的投注选项[[3,1,0],[3,1],[3,1,0],[1,0]]</param>
        /// <param name="_c_arr">选择的过关方法["2c1","3c1"]</param>
        /// <returns></returns>
        protected Int32 clacZyBetSum(List<String> _n_arr, List<String> _c_arr)
        {
            /** 定义返回值 */
            Int32 betSum = 0;
            /** 首先取出串长 */
            for (var i = 0; i < _c_arr.Count(); i++)
            {
                Int32 cLen = Convert.ToInt32(_c_arr[i].ToString().Split('*')[0]);
                List<String> oResult = findJcComb(_n_arr, _n_arr.Count(), cLen);
                for (var j = 0; j < oResult.Count(); j++)
                {
                    Int32 oSum = 1;
                    String[] oResulcount = oResult[j].Split('$');
                    for (var k = 0; k < oResulcount.Count(); k++)
                    {
                        oSum *= oResulcount[k].Split(',').Count();
                    }
                    betSum += oSum;
                }
            }
            return betSum;
        }

        /// <summary>
        /// 查找竞彩组合
        /// </summary>
        /// <param name="ops">选择的投注选项[[3,1,0],[3,1],[3,1,0],[1,0]]</param>
        /// <param name="n">选择场次数</param>
        /// <param name="k"></param>
        /// <returns></returns>
        protected static List<String> findJcComb(List<String> ops, Int32 n, Int32 k)
        {
            ArrayList buffer = new ArrayList();
            List<String> result = new List<String>();
            Int32 top = -1, tmp = 1;
            do
            {
                ++top;
                buffer.Add(tmp++);
            } while (top < k - 1);
            if (top == k - 1)
            {
                String tp = String.Empty;
                for (var i = 0; i < buffer.Count; i++)
                {
                    tp += ops[Convert.ToInt32(buffer[i]) - 1] + "$";
                }
                tp = tp.TrimEnd('$');
                result.Add(tp);
            }
            do
            {
                if (top == k - 1)
                {
                    do
                    {
                        tmp = Convert.ToInt32(buffer[top--]);
                    } while (tmp > n - (k - (top + 1)) && top > -1);
                }
                if (tmp <= n - (k - (top + 1)))
                {
                    do
                    {
                        buffer[++top] = ++tmp;
                    } while (top < k - 1);
                }
                if (top == k - 1)
                {
                    String tp = String.Empty;
                    for (var i = 0; i < buffer.Count; i++)
                    {
                        tp += ops[Convert.ToInt32(buffer[i]) - 1] + "$";
                    }
                    tp = tp.TrimEnd('$');
                    result.Add(tp);
                }
            } while (top > -1);
            return result;
        }


        /// <summary>
        /// 转换号码(每一位)
        /// </summary>
        /// <param name="Code">号码</param>
        /// <returns></returns>
        public string[] ParseOneBettCode(string Code)
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
        public string[] ParseTwoBettCode(string Code)
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
    }
}
