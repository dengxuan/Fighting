using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    [Table("BbcpLotteryPlayMappings")]
    public class LotteryPlayMapping : Entity<int>
    {
        /// <summary>
        /// <see cref="Text" /> 最大长度
        /// </summary>
        public const int MaxTextLength = 10;

        /// <summary>
        /// 彩种编号
        /// </summary>
        [Required]
        public int LotteryId { get; set; }

        /// <summary>
        /// 彩种 <see cref="Lotteries.Lottery"/>
        /// </summary>
        [ForeignKey("LotteryId")]
        public virtual Lottery Lottery { get; set; }

        /// <summary>
        /// 彩种玩法编号
        /// </summary>
        [Required]
        public int PlayId { get; set; }

        /// <summary>
        /// 彩种玩法 <see cref="Lotteries.LotteryPlay"/>
        /// </summary>
        [ForeignKey("PlayId")]
        public virtual LotteryPlay LotteryPlay { get; set; }

        public string Text { get; set; }
    }
}
