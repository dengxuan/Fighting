using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    [Table("BbcpPhaseBonuses")]
    public class LotteryPhaseBonus : Entity
    {
        /// <summary>
        /// 彩期编号
        /// </summary>
        [Required]
        public int LotteryPhaseId { get; set; }

        [ForeignKey("LotteryPhaseId")]
        public virtual LotteryPhase LotteryPhase { get; set; }

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
