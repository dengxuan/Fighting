using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Merchants
{
    [Table("BbcpChannelAccountDetails")]
    public class MerchanterAccountLogging : Entity<long>
    {
        /// <summary>
        /// 渠道编号
        /// </summary>
        public string MerchanterId { get; set; }


        /// <summary>
        /// 渠道 <see cref="Merchants.Merchanter"/>
        /// </summary>
        [ForeignKey("MerchanterId")]
        public Merchanter Merchanter { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryId { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        [ForeignKey("LotteryId")]
        public Lottery Lottery { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Column("Status")]
        public int OperationTypes { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public int Balance { get; set; }
    }
}
