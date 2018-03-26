using Fighting.Storaging.Entities.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.LotteryOrdering.Core.Entities.Merchantes
{
    [Table("BbcpOrders")]
    public class LotteryMerchanteOrder : Entity<string>
    {
        [MaxLength(32)]
        public override string Id { get => base.Id; set => base.Id = value; }

        /// <summary>
        /// 彩民编号
        /// </summary>
        public long LotteryBuyerId { get; set; }

        /// <summary>
        /// 投注渠道编号
        /// </summary>
        [Required]
        public string LvpVenderId { get; set; }

        /// <summary>
        /// 投注渠道订单号
        /// </summary>
        [Required]
        [Column("ChannelOrderId")]
        public string LvpOrderId { get; set; }

        /// <summary>
        /// 渠道用户编号
        /// </summary>
        public long? LvpUserId { get; set; }

        /// <summary>
        /// 出票渠道编号
        /// </summary>
        public string LdpVenderId { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        [Required]
        public int LotteryId { get; set; }

        /// <summary>
        /// 玩法编号
        /// </summary>
        [Required]
        public int LotteryPlayId { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public int? IssueNumber { get; set; }

        /// <summary>
        /// 投注码
        /// </summary>
        [Required]
        public string InvestCode { get; set; }

        /// <summary>
        /// 投注类型
        /// </summary>
        [Required]
        public bool InvestType { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        [Required]
        public int InvestCount { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        [Required]
        public int InvestTimes { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        [Required]
        public int InvestAmount { get; set; }


        /// <summary>
        /// 出票编号
        /// </summary>
        [Column("TicketId")]
        public string TicketedNumber { get; set; }

        /// <summary>
        /// 出票时间
        /// </summary>
        [Column("TicketTime")]
        public DateTime TicketedTime { get; set; }

        /// <summary>
        /// 出票赔率
        /// </summary>
        [Column("TicketOdds")]
        public string TicketedOdds { get; set; }

        /// <summary>
        /// 返奖金额
        /// </summary>
        public int? BonusAmount { get; set; }

        /// <summary>
        /// 税后返奖金额
        /// </summary>
        public int? AftertaxBonusAmount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
