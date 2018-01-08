using Baibaocp.Core.Entities;
using Baibaocp.Core.Extensions;
using Baibaocp.LotteryAwardCalculator.Internal;
using Baibaocp.LotteryOrdering.Messages;
using Dapper;
using Fighting.Caching.Abstractions;
using Fighting.Storage;
using Microsoft.Extensions.Logging;
using Pomelo.Data.MySql;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace Baibaocp.LotteryAwardCalculator.Abstractions
{
    public class Calculator : ICalculator
    {
        private readonly StorageOptions _options;

        private readonly ICacheManager _cacheManager;

        private readonly ILogger<Calculator> _logger;

        protected NameValueCollection Nvc { get; private set; }

        public Calculator(StorageOptions options, ICacheManager cacheManager, ILogger<Calculator> logger)
        {
            _options = options;
            _cacheManager = cacheManager;
            _logger = logger;
            Nvc = new NameValueCollection();
        }

        public LotterySportsMatchResultEntity SelectXbZcResult(string eventId)
        {
            string sql = "select `Id`,`Date`,`Week`,`PlayId`,`RqspfRateCount`,`Score`,`HalfScore` from `BbcpZcEvents` where `Id` = @EventId;";
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                string id = string.Empty;
                LotterySportsMatchResultEntity result = connection.QueryFirst<LotterySportsMatchResultEntity>(sql, new { @EventId = eventId });
                if (result != null)
                {
                    if (result.Score == "")
                    {
                        result.Cancel = 0;
                    }
                    else if (result.Score == "-1:-1")
                    {
                        result.Cancel = 1;
                    }
                }
                return result;
            }
        }

        public LotterySportsMatchResultEntity SelectXbLcResult(string eventid)
        {
            string sql = "select `Id`,`matchId`,`Date`,`Week`,`PlayId`,`RfsfRateCount`,`BaseCount`,`Score` from `BbcpLcEvents` where `Id` = @eventid and `Score` != ''";
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                string id = string.Empty;
                using (IDataReader reader = connection.ExecuteReader(sql, new object[] { eventid }))
                {
                    if (reader.Read())
                    {
                        LotterySportsMatchResultEntity result = new LotterySportsMatchResultEntity
                        {
                            Id = reader.GetInt64(0),
                            MatchId = reader.GetString(1),
                            Date = reader.GetString(2),
                            Week = reader.GetValue(3) as int?,
                            PlayId = reader.GetString(4),
                            RqspfRateCount = reader.GetString(5),
                            Bases = reader.GetString(6),
                            Score = reader.GetString(7)
                        };
                        if (reader.GetString(7) == "")
                        {
                            result.Cancel = 0;
                        }
                        else if (reader.GetString(7) == "-1:-1")
                        {
                            result.Cancel = 1;
                        }

                        return result;
                    }
                }
            }
            return null;
        }

        protected LotterySportsMatchResultEntity GetEventResult(long eventId, int type)
        {
            ICache cacher = _cacheManager.GetCache("Events");
            LotterySportsMatchResultEntity result = cacher.Get(eventId.ToString(), (k) =>
           {
               LotterySportsMatchResultEntity eventresult = new LotterySportsMatchResultEntity();
               if (type == 20200)
               {
                   eventresult = this.SelectXbZcResult(k);
               }
               else if (type == 20400)
               {
                   eventresult = this.SelectXbLcResult(k);
               }
               return eventresult;
           }) as LotterySportsMatchResultEntity;
            return result;
        }

        /// <summary>
        /// 竞彩取得赛果
        /// </summary>
        /// <param name="eventid">赛事ID</param>
        /// <returns>赛果</returns>
        protected string GetScoreResult(string eventdata, string lotid)
        {
            _logger.LogInformation("彩种：{0} 算奖 EventData: {1}", lotid, eventdata);
            long eventid = 0;
            string lotteryid = string.Empty;

            string[] eventarr = eventdata.Split('@');
            if (lotid == "20205" || lotid == "20405")
            {
                string[] eventlist = eventarr[0].Split('-');
                eventid = Convert.ToInt64(eventlist[0]);
                lotteryid = eventlist[1];
            }
            else
            {
                eventid = Convert.ToInt64(eventarr[0]);
                lotteryid = lotid;
            }
            string vsresult = string.Empty;
            if (lotteryid == "20201" || lotteryid == "20202" || lotteryid == "20203" || lotteryid == "20204" || lotteryid == "20206")
            {
                vsresult = this.GetZcResult(Convert.ToInt32(eventarr[1]), eventid, lotteryid);
            }
            else if (lotteryid == "20401" || lotteryid == "20402" || lotteryid == "20403" || lotteryid == "20404")
            {
                vsresult = this.GetLcResult(Convert.ToDouble(eventarr[1]), eventid, lotteryid);
            }
            return vsresult;
        }


        /// <summary>
        /// 计算足彩开奖结果
        /// </summary>
        /// <param name="let"></param>
        /// <param name="eventid"></param>
        /// <param name="lotteryid"></param>
        /// <returns></returns>
        private string GetZcResult(Int32 let, long eventid, string lotteryid)
        {
            string vsresult = string.Empty;
            String score = string.Empty;
            String haltscore = string.Empty;
            if (this.Nvc.Get(eventid.ToString()) != null)
            {
                score = this.Nvc.Get(eventid.ToString());
                haltscore = this.Nvc.Get("half" + eventid.ToString());
            }
            else
            {
                LotterySportsMatchResultEntity result = GetEventResult(eventid, 20200);
                if (result != null)
                {
                    score = result.Score;
                    haltscore = result.HalfScore;
                    this.Nvc.Add(result.Id.ToString(), score);
                    this.Nvc.Add("half" + result.Id.ToString(), haltscore);
                }
                else
                {
                    score = string.Empty;
                    haltscore = string.Empty;
                }
            }
            if (score != string.Empty)
            {
                string[] vs = score.Split(':');
                string[] haltvs = haltscore.Split(':');
                int first = int.Parse(vs[0]);
                int second = int.Parse(vs[1]);
                int haltfist = int.Parse(haltvs[0]);
                int haltsecond = int.Parse(haltvs[1]);
                if (lotteryid == "20201" || lotteryid == "20206")
                {
                    if (first + let > second)
                    {
                        vsresult = "3";
                    }
                    if (first + let == second)
                    {
                        vsresult = "1";
                    }
                    if (first + let < second)
                    {
                        vsresult = "0";
                    }
                }
                else if (lotteryid == "20202")
                {
                    string[] bifen = this.Bifen();
                    vsresult = first.ToString() + "" + second.ToString();
                    if (!bifen.Contains(vsresult))
                    {
                        if (first > second)
                        {
                            vsresult = "90";
                        }
                        if (first == second)
                        {
                            vsresult = "99";
                        }
                        if (first < second)
                        {
                            vsresult = "09";
                        }
                    }
                }
                else if (lotteryid == "20203")
                {
                    Int32 sum = first + second;
                    vsresult = sum.ToString();
                }
                else if (lotteryid == "20204")
                {
                    String haltvaresult = String.Empty;
                    if (first + let > second)
                    {
                        vsresult = "3";
                    }
                    if (first + let == second)
                    {
                        vsresult = "1";
                    }
                    if (first + let < second)
                    {
                        vsresult = "0";
                    }
                    if (haltfist > haltsecond)
                    {
                        vsresult = "3" + vsresult;
                    }
                    if (haltfist == haltsecond)
                    {
                        vsresult = "1" + vsresult;
                    }
                    if (haltfist < haltsecond)
                    {
                        vsresult = "0" + vsresult;
                    }
                }

                if (first == -1 && second == -1)
                {
                    vsresult = "-1";
                }
            }
            else
            {
                vsresult = null;
            }
            return vsresult;
        }


        /// <summary>
        /// 计算篮彩开奖结果
        /// </summary>
        /// <param name="let"></param>
        /// <param name="eventid"></param>
        /// <param name="lotteryid"></param>
        /// <returns></returns>
        private String GetLcResult(Double let, long eventid, String lotteryid)
        {
            String vsresult = String.Empty;
            String score = string.Empty;
            if (this.Nvc.Get(eventid.ToString()) != null)
            {
                score = this.Nvc.Get(eventid.ToString());
            }
            else
            {
                LotterySportsMatchResultEntity result = GetEventResult(eventid, 20400);
                if (result != null)
                {
                    score = result.Score;
                    this.Nvc.Add(result.Id.ToString(), score);
                }
                else
                {
                    score = string.Empty;
                }
            }
            if (score != string.Empty)
            {
                //data.score = "102:105";
                string[] vs = score.Split(':');
                int first = int.Parse(vs[1]);//主队
                int second = int.Parse(vs[0]);//客队
                if (lotteryid == "20401" || lotteryid == "20402")
                {
                    if (first + let > second)
                    {
                        vsresult = "3";
                    }
                    if (first + let < second)
                    {
                        vsresult = "0";
                    }
                }
                else if (lotteryid == "20403")
                {
                    Int32 diff = Math.Abs(first - second);
                    if (diff >= 1 && 5 >= diff)
                    {
                        vsresult = first > second ? "01" : "11";
                    }
                    if (diff >= 6 && 10 >= diff)
                    {
                        vsresult = first > second ? "02" : "12";
                    }
                    if (diff >= 11 && 15 >= diff)
                    {
                        vsresult = first > second ? "03" : "13";
                    }
                    if (diff >= 16 && 20 >= diff)
                    {
                        vsresult = first > second ? "04" : "14";
                    }
                    if (diff >= 21 && 25 >= diff)
                    {
                        vsresult = first > second ? "05" : "15";
                    }
                    if (diff >= 26)
                    {
                        vsresult = first > second ? "06" : "16";
                    }
                }
                else if (lotteryid == "20404")
                {
                    Int32 total = first + second;
                    if (let > total)
                    {
                        vsresult = "2";
                    }
                    else
                    {
                        vsresult = "1";
                    }
                }
                if (first == -1 && second == -1)
                {
                    vsresult = "-1";
                }
            }
            else
            {
                vsresult = null;
            }
            return vsresult;
        }

        protected string[] Bifen()
        {
            string[] bifen = new string[] {"10","20","21","30","31","32","40","41","42","50","51","52",
                                           "01","02","12","03","13","23","04","14","24","05","15","25",
                                           "00","11","22","33"};
            return bifen;
        }

        public Handle Calculate(TicketedMessage ticketedMessage)
        {
            _logger.LogInformation("开始算奖: 订单号 {0} 投注码: {1} 出票赔率：{2}", ticketedMessage.LvpOrder.LvpOrderId, ticketedMessage.LvpOrder.InvestCode, ticketedMessage.TicketOdds);
            string ticketOdds = ticketedMessage.TicketOdds;
            string[] code = ticketOdds.TrimEnd('^').Split('^');
            int sale = int.Parse($"N{ticketedMessage.LvpOrder.LotteryPlayId}".ToJingcaiLottery());
            int count = 0;
            int vscount = 0;
            for (int j = 0; j < code.Length; j++)
            {
                string[] eventdata = code[j].Split('|');
                String vsreult = this.GetScoreResult(eventdata[0].ToString(), ticketedMessage.LvpOrder.LotteryId.ToString());
                string[] odds = eventdata[1].Split('#');
                if (vsreult != null)
                {
                    vscount = vscount + 1;
                    if (vsreult == "-1")
                    {
                        count = count + 1;
                    }
                    else
                    {
                        for (int x = 0; x < odds.Length - 1; x++)
                        {
                            String[] newodds = odds[x].Split('*');
                            if (newodds[0] == vsreult)
                            {
                                count = count + 1;
                            }
                        }
                    }
                }
            }
            if (vscount >= sale) //赛事完成数量与串关数量比对
            {
                if (count >= sale)//中奖赛事与串关数量
                {
                    return Handle.Winner;
                }
                else
                {
                    return Handle.Losing;
                }
            }
            else
            {
                return Handle.Waiting;
            }
        }
    }
}
