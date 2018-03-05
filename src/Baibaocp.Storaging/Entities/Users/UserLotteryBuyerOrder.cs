using Baibaocp.Storaging.Entities.Lotteries;
using Baibaocp.Storaging.Entities.Merchants;
using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Users
{
    [Table("BbcpUserLotteryBuyerOrders")]
    public class UserLotteryBuyerOrder : Entity<long>
    {
        [Required]
        public long LotteryBuyerId { get; set; }

        [ForeignKey("LotteryBuyerId")]
        public virtual UserLotteryBuyer BbcpUserLotteryBuyer { get; set; }

        /// <summary> 
        /// 销售渠道用户编号
        /// </summary>
        [Required]
        public long ResellerUserId { get; set; }

        /// <summary> 
        /// 销售渠道编号
        /// </summary>
        [Required]
        public int LotteryResellerId { get; set; }

        [ForeignKey("LotteryResellerId")]
        public virtual Merchanter LotteryReseller { get; set; }

        /// <summary> 
        /// 出票渠道编号
        /// </summary>
        public int LotterySupplierId { get; set; }

        [ForeignKey("LotterySupplierId")]
        public virtual Merchanter LotterySupplier { get; set; }

        /// <summary>
        /// 彩种id
        /// </summary>
        [Required]
        public int LotteryId { get; set; }


        [ForeignKey("LotteryId")]
        public virtual Lottery Lottery { get; set; }

        /// <summary>
        /// 玩法id
        /// </summary>
        [Required]
        public int LotteryPlayId { get; set; }


        [ForeignKey("LotteryPlayId")]
        public virtual LotteryPlay LotteryPlay { get; set; }

        [Required]
        public int? IssueNumber { get; set; }

        /// <summary>
        /// 购买奖号
        /// </summary>
        public string InvestCode { get; set; }

        /// <summary>
        /// 出票赔率
        /// </summary>
        public string TicketOdds { get; set; }

        /// <summary>
        /// 投注类型
        /// </summary>
        public int InvestType { get; set; }

        /// <summary>
        /// 投注数
        /// </summary>
        [Required]
        public int InvestCount { get; set; }

        /// <summary>
        /// 投注倍数
        /// </summary>
        public int InvestTimes { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public int InvestAmount { get; set; }

        /// <summary>
        /// 返奖金额
        /// </summary>
        public int BonusAmount { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 出票是否通知
        /// </summary>
        public int? IsTicketNotify { get; set; }

        /// <summary>
        /// 返奖是否通知
        /// </summary>
        public int? IsBonusNotify { get; set; }

    }
}
