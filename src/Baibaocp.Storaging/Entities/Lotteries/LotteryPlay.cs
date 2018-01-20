using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    /// <summary>
    /// 彩种玩法
    /// </summary>
    [Table("BbcpLotteryPlays")]
    public class LotteryPlay : Entity<int>
    {
        /// <summary>
        /// <see cref="Text"/> 最大长度
        /// </summary>
        public const int MaxTextLength = 10;

        /// <summary>
        /// 玩法<see cref="LotteryPlayMapping" />映射彩种集合 <see cref="ICollection{T}" />
        /// </summary>
        [ForeignKey("LotteryPlayId")]
        public ICollection<LotteryPlayMapping> LotteryPlayMappings { get; set; }

        /// <summary>
        /// 玩法名称
        /// </summary>
        public string Text { get; set; }
    }
}
