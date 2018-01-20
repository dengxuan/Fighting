using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    /// <summary>
    /// 彩种类别 <see cref="Entity"/> <seealso cref="Entity{TPrimaryKey}"/>
    /// </summary>
    [Table("BbcpLotteryCategories")]
    public class LotteryCategory : Entity<short>
    {
        /// <summary>
        /// 彩种类别名称 <see cref="Text"/> 的最大长度
        /// </summary>
        public const int MaxTextLength = 20;

        /// <summary>
        /// 彩种类型编号 <see cref="int"/>
        /// </summary>
        [Required]
        public short LotteryTypeId { get; set; }

        /// <summary>
        /// 彩种类型 <see cref="Core.Lotteries.LotteryType"/>
        /// </summary>
        [ForeignKey("LotteryTypeId")]
        public LotteryType LotteryType { get; set; }

        /// <summary>
        /// 彩种<see cref="Lottery"/> 的集合 <see cref="ICollection{T}"/>
        /// </summary>
        [ForeignKey("BbcpLotteryId")]
        public ICollection<Lottery> Lotteries { get; set; }

        /// <summary>
        /// 彩种类别名称 <see cref="string"/>, 最大长度为 <see cref="MaxTextLength"/>
        /// </summary>
        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }
    }
}
