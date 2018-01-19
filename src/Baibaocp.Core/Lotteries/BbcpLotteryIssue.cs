using Fighting.Storaging.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Lotteries
{
    [Table("BbcpIssues")]
    public class BbcpLotteryIssue : Entity<long>
    {
        /// <summary>
        /// 彩种
        /// </summary>
        [Required]
        public int LotteryId { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        [ForeignKey("LotteryId")]
        public virtual BbcpLottery Lottery { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        [Required]
        public int IssueNumber { get; set; }

        /// <summary>
        /// 期扩展数据
        /// </summary>
        public string IssueExtdatas { get; set; }

        /// <summary>
        /// 开期时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 开始销售时间
        /// </summary>
        [Required]
        public DateTime StartSaleTime { get; set; }

        /// <summary>
        /// 结束销售时间
        /// </summary>
        [Required]
        public DateTime EndSaleTime { get; set; }

        /// <summary>
        /// 结期时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 开奖号
        /// </summary>
        public string DrawNumber { get; set; }

        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime? DrawTime { get; set; }

        /// <summary>
        /// 开奖来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 是否同步
        /// </summary>
        [Required]
        public bool IsAsync { get; set; }

        /// <summary>
        /// 开奖状态
        /// </summary>
        [Required]
        public int Status { get; set; }

        [ForeignKey("IssueId")]
        public virtual ICollection<BbcpLotteryIssueBonus> LotteryIssueBonuses { get; set; }

        /// <summary>
        /// 总销售额
        /// </summary>
        public long? TotalSaleAmount { get; set; }

        /// <summary>
        ///总返奖
        /// </summary>
        public long? TotalAwardAmount { get; set; }

        /// <summary>
        ///奖池
        /// </summary>
        public long? AwardPoolAmount { get; set; }

        /// <summary>
        /// 停止下单时间
        /// </summary>
        public DateTime? EndOrderTime { get; set; }
    }
}
