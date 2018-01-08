using Fighting.Storage.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Baibaocp.Core.Entities
{
    public class LotterySportsMatchEntity : Entity<long>
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Required]
        public string Date { get; set; }

        /// <summary>
        /// 周
        /// </summary>
        [Required]
        public int Week { get; set; }

        /// <summary>
        /// 赛事编号
        /// </summary>
        [Required]
        public string PlayId { get; set; }

        /// <summary>
        /// 联赛
        /// </summary>
        [Required]
        public string League { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [Required]
        public string Color { get; set; }

        /// <summary>
        /// 主队
        /// </summary>
        [Required]
        public string HostTeam { get; set; }

        /// <summary>
        /// 客队
        /// </summary>
        [Required]
        public string VisitTeam { get; set; }

        /// <summary>
        /// 开赛时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结赛时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 销售状态
        /// </summary>
        [Required]
        public int SaleStatus { get; set; }

        /// <summary>
        /// 比分
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// 半全场比分
        /// </summary>
        public string HalfScore { get; set; }

        /// <summary>
        /// 是否支持胜平负单关
        /// </summary>
        public bool SpfIsSupSinglePass { get; set; }

        /// <summary>
        /// 是否支持让球胜平负单关
        /// </summary>
        public bool RqspfIsSupSinglePass { get; set; }

        /// <summary>
        /// 是否支持比分胜平负
        /// </summary>
        public bool ScoreIsSupSinglePass { get; set; }

        /// <summary>
        /// 总进球是否支持单关
        /// </summary>
        public bool TotalGoalsIsSupSinglePass { get; set; }

        /// <summary>
        /// 总比分是否支持半全场单关
        /// </summary>
        public bool HalfScoreIsSupSinglePass { get; set; }

        /// <summary>
        ///让球数
        /// </summary>
        public int RqspfRateCount { get; set; }

        public decimal? SpfOdds3 { get; set; }

        public decimal? SpfOdds1 { get; set; }

        public decimal? SpfOdds0 { get; set; }

        public decimal? RqspfOdds3 { get; set; }

        public decimal? RqspfOdds1 { get; set; }

        public decimal? RqspfOdds0 { get; set; }

        public decimal? ScoreOdds10 { get; set; }

        public decimal? ScoreOdds20 { get; set; }

        public decimal? ScoreOdds21 { get; set; }

        public decimal? ScoreOdds30 { get; set; }

        public decimal? ScoreOdds31 { get; set; }

        public decimal? ScoreOdds32 { get; set; }

        public decimal? ScoreOdds40 { get; set; }

        public decimal? ScoreOdds41 { get; set; }

        public decimal? ScoreOdds42 { get; set; }

        public decimal? ScoreOdds50 { get; set; }

        public decimal? ScoreOdds51 { get; set; }

        public decimal? ScoreOdds52 { get; set; }

        public decimal? ScoreOdds90 { get; set; }

        public decimal? ScoreOdds00 { get; set; }

        public decimal? ScoreOdds11 { get; set; }

        public decimal? ScoreOdds22 { get; set; }

        public decimal? ScoreOdds33 { get; set; }

        public decimal? ScoreOdds99 { get; set; }

        public decimal? ScoreOdds01 { get; set; }

        public decimal? ScoreOdds02 { get; set; }

        public decimal? ScoreOdds03 { get; set; }

        public decimal? ScoreOdds12 { get; set; }

        public decimal? ScoreOdds13 { get; set; }

        public decimal? ScoreOdds23 { get; set; }

        public decimal? ScoreOdds04 { get; set; }

        public decimal? ScoreOdds14 { get; set; }

        public decimal? ScoreOdds24 { get; set; }

        public decimal? ScoreOdds05 { get; set; }

        public decimal? ScoreOdds15 { get; set; }

        public decimal? ScoreOdds25 { get; set; }

        public decimal? ScoreOdds09 { get; set; }

        public decimal? TotalGoalsOdds0 { get; set; }

        public decimal? TotalGoalsOdds1 { get; set; }

        public decimal? TotalGoalsOdds2 { get; set; }

        public decimal? TotalGoalsOdds3 { get; set; }

        public decimal? TotalGoalsOdds4 { get; set; }

        public decimal? TotalGoalsOdds5 { get; set; }

        public decimal? TotalGoalsOdds6 { get; set; }

        public decimal? TotalGoalsOdds7 { get; set; }

        public decimal? HalfScore33 { get; set; }

        public decimal? HalfScore31 { get; set; }

        public decimal? HalfScore30 { get; set; }

        public decimal? HalfScore13 { get; set; }

        public decimal? HalfScore11 { get; set; }

        public decimal? HalfScore10 { get; set; }

        public decimal? HalfScore03 { get; set; }

        public decimal? HalfScore01 { get; set; }

        public decimal? HalfScore00 { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
