using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Merchants
{
    [Table("BbcpChannelLotteryMappings")]
    public class MerchanterLotteryMapping : Entity
    {
        /// <summary>
        /// 投注渠道编号
        /// </summary
        [Column("ChannelId")]
        public string LvpMerchanterId { get; set; }

        /// <summary>
        /// 出票渠道 <see cref="Merchants.Merchanter"/>
        /// </summary>
        [ForeignKey("LvpMerchanterId")]
        public virtual Merchanter LvpMerchanter { get; set; }

        /// <summary>
        /// 出票渠道编号
        /// </summary
        [Column("LdpVenderId")]
        public string LdpMerchanterId { get; set; }

        /// <summary>
        /// 出票渠道 <see cref="Merchants.Merchanter"/>
        /// </summary>
        [ForeignKey("LdpMerchanterId")]
        public virtual Merchanter LdpMerchanter { get; set; }

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
        /// 是否启用
        /// </summary>
        [NotMapped]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否独占
        /// </summary>
        [NotMapped]
        public bool IsMonopolized { get; set; }
    }
}
