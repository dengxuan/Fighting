using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Lotteries
{
    /// <summary>
    /// 彩种玩法
    /// </summary>
    [Table("BbcpLotteryPlays")]
    public class BbcpLotteryPlay : Entity<int>
    {
        /// <summary>
        /// <see cref="Text"/> 最大长度
        /// </summary>
        public const int MaxTextLength = 10;

        /// <summary>
        /// 玩法<see cref="BbcpLotteryPlayMapping" />映射彩种集合 <see cref="ICollection{T}" />
        /// </summary>
        [ForeignKey("LotteryPlayId")]
        public ICollection<BbcpLotteryPlayMapping> LotteryPlayMappings { get; set; }

        /// <summary>
        /// 玩法名称
        /// </summary>
        public string Text { get; set; }
    }
}
