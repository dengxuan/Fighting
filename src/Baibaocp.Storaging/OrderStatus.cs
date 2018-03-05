namespace Baibaocp.Storaging.Entities
{
    public enum OrderStatus
    {
        /// <summary>
        /// 接单成功
        /// </summary>
        Succeed = 1000,

        /// <summary>
        /// 等待出票
        /// </summary>
        Ticketing = 2000,

        /// <summary>
        /// 出票失败
        /// </summary>
        TicketFailed = 3000,

        /// <summary>
        /// 票过期，未投注
        /// </summary>
        TicketNotSend = 3001,

        /// <summary>
        /// 上游系统未接单
        /// </summary>
        TicketNotRecv = 3002,

        /// <summary>
        /// 等待开奖
        /// </summary>
        TicketDrawing = 4000,

        /// <summary>
        /// 已中奖
        /// </summary>
        TicketWinning = 4001,

        /// <summary>
        /// 未中奖
        /// </summary>
        TicketLosing = 4002,

        /// <summary>
        /// 已返奖
        /// </summary>
        TicketGrantAward = 4003,

        /// <summary>
        /// 中奖未查询
        /// </summary>
        WinningNotQuery = 4004,
    }
}
