using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Entities
{
    public class LotteryEntity : Entity<int>
    {
        /// <summary>
        /// 彩种名称 <see cref="Text"/> 的最大长度
        /// </summary>
        public const int MaxTextLength = 10;

        /// <summary>
        /// 彩种分类编号 <see cref="int"/>
        /// </summary>
        [Required]
        public int LotteryCategoryId { get; set; }

        /// <summary>
        /// 彩种分类 <see cref="LotteryCategory"/>
        /// </summary>
        [ForeignKey("LotteryCategoryId")]
        public LotteryCategoryEntity LotteryCategory { get; set; }

        /// <summary>
        /// 玩法<see cref="BbcpLotteryPlayMapping" />映射彩种集合 <see cref="ICollection{T}" />
        /// </summary>
        [ForeignKey("LotteryId")]
        public ICollection<LotteryPlayMappingEntity> LotteryPlayMappings { get; set; }

        /// <summary>
        ///// 设置ID不自增
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public override int Id { get; set; }

        /// <summary>
        /// 彩种前缀，用来划分账号区域
        /// </summary>
        [Required]
        public int Prefix { get; set; }

        /// <summary>
        /// 彩种名称
        /// </summary>
        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }
    }
}
