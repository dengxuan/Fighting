using Baibaocp.Core.Lotteries;
using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Users
{
    [Table("BbcpOrders")]
    public class BbcpUserOrders : Entity<string>
    {
        [Required]
        public long LotteryBuyerId { get; set; }

        [ForeignKey("LotteryBuyerId")]
        public virtual BbcpUserLotteryBuyer BbcpUserLotteryBuyer { get; set; }

        /// <summary> 
        /// 渠道用户ID
        /// </summary>
        [Required]
        public long LvpUserId { get; set; }

        /// <summary> 
        /// 接票渠道Id（全球播）
        /// </summary>
        [Required]
        public string LvpVenderId { get; set; }

        [ForeignKey("LvpVenderId")]
        public virtual BbcpChannel LvpVender { get; set; }

        /// <summary> 
        /// 出票渠道Id
        /// </summary>
        public string LdpVenderId { get; set; }

        [ForeignKey("LdpVenderId")]
        public virtual BbcpChannel LdpVender { get; set; }

        /// <summary>
        /// 彩种id
        /// </summary>
        [Required]
        public int LotteryId { get; set; }


        [ForeignKey("LotteryId")]
        public virtual BbcpLottery Lottery { get; set; }

        /// <summary>
        /// 玩法id
        /// </summary>
        [Required]
        public int LotteryPlayId { get; set; }


        [ForeignKey("LotteryPlayId")]
        public virtual BbcpLotteryPlay LotteryPlay { get; set; }
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
