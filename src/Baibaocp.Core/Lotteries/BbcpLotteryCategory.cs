using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Lotteries
{
    /// <summary>
    /// 彩种类别 <see cref="Entity"/> <seealso cref="Entity{TPrimaryKey}"/>
    /// </summary>
    [Table("BbcpLotteryCategories")]
    public class BbcpLotteryCategory : Entity<int>
    {
        /// <summary>
        /// 彩种类别名称 <see cref="Text"/> 的最大长度
        /// </summary>
        public const int MaxTextLength = 20;

        /// <summary>
        /// 彩种类型编号 <see cref="int"/>
        /// </summary>
        [Required]
        public int LotteryTypeId { get; set; }

        /// <summary>
        /// 彩种类型 <see cref="BbcpLotteryType"/>
        /// </summary>
        [ForeignKey("LotteryTypeId")]
        public BbcpLotteryType LotteryType { get; set; }

        /// <summary>
        /// 彩种<see cref="BbcpLottery"/> 的集合 <see cref="ICollection{T}"/>
        /// </summary>
        [ForeignKey("BbcpLotteryId")]
        public ICollection<BbcpLottery> Lotteries { get; set; }

        /// <summary>
        /// 彩种类别名称 <see cref="string"/>, 最大长度为 <see cref="MaxTextLength"/>
        /// </summary>
        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }
    }
}
