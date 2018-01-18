using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Entities
{
    public class LotteryCategoryEntity : Entity<int>
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
        /// 彩种类型 <see cref="LotteryType"/>
        /// </summary>
        [ForeignKey("LotteryTypeId")]
        public LotteryTypeEntity LotteryType { get; set; }

        /// <summary>
        /// 彩种<see cref="LotteryEntity"/> 的集合 <see cref="ICollection{T}"/>
        /// </summary>
        [ForeignKey("LotteryId")]
        public ICollection<LotteryEntity> Lotteries { get; set; }

        /// <summary>
        /// 彩种类别名称 <see cref="string"/>, 最大长度为 <see cref="MaxTextLength"/>
        /// </summary>
        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }
    }
}
