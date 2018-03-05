using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Merchants
{
    [Table("BbcpLotteryMerchanterMappings")]
    public class MerchanterLotteryMapping : Entity
    {
        /// <summary>
        /// 渠道编号
        /// </summary
        public int MerchanterId { get; set; }

        /// <summary>
        /// 渠道 <see cref="Merchants.Merchanter"/>
        /// </summary>
        [ForeignKey("MerchanterId")]
        public virtual Merchanter Merchanter { get; set; }

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
        /// 佣金比例
        /// </summary>
        public decimal CommissionRate { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string NoticeAddress { get; set; }
    }
}
