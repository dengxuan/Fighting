using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Lotteries
{
    [Table("BbcpLotteryPlayMappings")]
    public class BbcpLotteryPlayMapping : Entity<int>
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
        /// 彩种 <see cref="BbcpLottery"/>
        /// </summary>
        [ForeignKey("LotteryId")]
        public virtual BbcpLottery Lottery { get; set; }

        /// <summary>
        /// 彩种玩法编号
        /// </summary>
        [Required]
        public int PlayId { get; set; }

        /// <summary>
        /// 彩种玩法 <see cref="BbcpLotteryPlay"/>
        /// </summary>
        [ForeignKey("PlayId")]
        public virtual BbcpLotteryPlay LotteryPlay { get; set; }

        public string Text { get; set; }
    }
}
