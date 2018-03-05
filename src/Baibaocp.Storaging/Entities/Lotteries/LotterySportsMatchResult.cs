using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.Storaging.Entities.Entities
{
    public class LotterySportsMatchResult
    {

        /// <summary>
        /// 赛事ID
        /// </summary>
        public long Id { get; set; }

        public string MatchId { get; set; }

        /// <summary>
        /// 比赛取消
        /// </summary>
        public int Cancel { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 周数
        /// </summary>
        public int? Week { get; set; }

        /// <summary>
        /// 场次
        /// </summary>
        public string PlayId { get; set; }

        /// <summary>
        /// 让球/让分
        /// </summary>
        public string RqspfRateCount { get; set; }

        /// <summary>
        /// 预设总分
        /// </summary>
        public string Bases { get; set; }

        /// <summary>
        /// 全场比分
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// 上半场比分
        /// </summary>
        public string HalfScore { get; set; }
    }
}
