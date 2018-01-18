using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Entities
{
    public class LotteryIssueBonusEntity : Entity<int>
    {
        /// <summary>
        /// 期号
        /// </summary>
        [Required]
        public int LotteryIssueId { get; set; }

        [ForeignKey("LotteryIssueId")]
        public virtual LotteryIssueEntity LotteryIssue { get; set; }

        /// <summary>
        /// 奖级
        /// </summary> 
        public int BonusLevel { get; set; }

        /// <summary>
        /// 奖级描述
        /// </summary>
        public string BonusName { get; set; }

        /// <summary>
        /// 奖金
        /// </summary>
        public int BonusAmount { get; set; }

        /// <summary>
        /// 奖级中奖个数
        /// </summary>
        public int WinnerCount { get; set; }

        /// <summary>
        /// 奖金个数
        /// </summary>
        public int TotalWinnerCount { get; set; }
    }
}
