using Baibaocp.LotteryAwardCalculator.Internal;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities.Entities;
using Baibaocp.Storaging.Entities.Extensions;
using Dapper;
using Fighting.Caching.Abstractions;
using Fighting.Storaging;
using Microsoft.Extensions.Logging;
using Pomelo.Data.MySql;
using System;
using System.Linq;

namespace Baibaocp.LotteryAwardCalculator.Abstractions
{
    public class Calculator : ICalculator
    {
        private readonly StorageOptions _options;

        private readonly ICacheManager _cacheManager;

        private readonly ILogger<Calculator> _logger;

        public Calculator(StorageOptions options, ICacheManager cacheManager, ILogger<Calculator> logger)
        {
            _options = options;
            _cacheManager = cacheManager;
            _logger = logger;
        }

        public LotterySportsMatchResult SelectZcResult(string eventId)
        {
            string sql = "select `Id`,`Date`,`Week`,`PlayId`,`RqspfRateCount`,`Score`,`HalfScore` from `BbcpZcEvents` where `Id` = @EventId;";
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                string id = string.Empty;
                LotterySportsMatchResult result = connection.QueryFirst<LotterySportsMatchResult>(sql, new { @EventId = eventId });
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

        protected LotterySportsMatchResult GetEventResult(long eventId)
        {
            ICache cacher = _cacheManager.GetCache("Events");
            LotterySportsMatchResult result = cacher.Get(eventId.ToString(), (k) =>
            {
                LotterySportsMatchResult eventresult = this.SelectZcResult(k);
                return eventresult;
            });
            return result;
        }

        /// <summary>
        /// 竞彩取得赛果
        /// </summary>
        /// <param name="eventid">赛事ID</param>
        /// <returns>赛果</returns>
        protected string GetScoreResult(string eventdata, string lotid)
        {
            long eventid = 0;
            string lotteryid = string.Empty;

            string[] eventarr = eventdata.Split('@');
            if (lotid == "20205")
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
            LotterySportsMatchResult result = GetEventResult(eventid);
            if (result != null)
            {
                score = result.Score;
                haltscore = result.HalfScore;
            }
            if (!string.IsNullOrEmpty(score) && !string.IsNullOrEmpty(haltscore))
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
            _logger.LogInformation($"赛事 {eventid} 半场比分 {haltscore} 比分 {score} 结果 {vsresult}");
            return vsresult;
        }

        protected string[] Bifen()
        {
            string[] bifen = new string[] {"10","20","21","30","31","32","40","41","42","50","51","52",
                                           "01","02","12","03","13","23","04","14","24","05","15","25",
                                           "00","11","22","33"};
            return bifen;
        }

        public Handle Calculate(LdpTicketedMessage ticketedMessage)
        {
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
            Handle handle = Handle.Waiting;
            //赛事完成数量与串关数量比对
            if (vscount >= sale)
            {
                //中奖赛事与串关数量
                if (count >= sale)
                {
                    handle = Handle.Winner;
                }
                else
                {
                    handle = Handle.Losing;
                }
            }
            _logger.LogInformation($"算奖 订单号: {ticketedMessage.LvpOrder.LvpOrderId} 彩种: {ticketedMessage.LvpOrder.LotteryId} 投注码: {ticketedMessage.LvpOrder.InvestCode} 出票赔率：{ticketedMessage.TicketOdds} 完成数量：{vscount} 中奖赛事数量：{count} 计算结果: {handle}");

            return handle;
        }
    }
}
