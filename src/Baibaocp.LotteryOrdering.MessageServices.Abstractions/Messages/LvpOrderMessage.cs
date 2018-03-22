namespace Baibaocp.LotteryOrdering.MessageServices.Messages
{
    public class LvpOrderMessage
    {
        /// <summary>
        /// 下游渠道订单号
        /// </summary>
        public string LvpOrderId { get; set; }

        /// <summary>
        /// 渠道编号
        /// </summary>
        public string LvpVenderId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long? LvpUserId { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        public int LotteryId { get; set; }

        /// <summary>
        /// 玩法编号
        /// </summary>
        public int LotteryPlayId { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public int? IssueNumber { get; set; }

        /// <summary>
        /// 投注码
        /// </summary>
        public string InvestCode { get; set; }

        /// <summary>
        /// 投注类型
        /// </summary>
        public bool InvestType { get; set; }

        /// <summary>
        /// 注数
        /// </summary>
        public short InvestCount { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public byte InvestTimes { get; set; }

        /// <summary>
        /// 投注金额
        /// </summary>
        public int InvestAmount { get; set; }
    }
}
