using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Fighting.ApplicationServices.Abstractions;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.ApplicationServices.Abstractions
{
    public interface IOrderingApplicationService : IApplicationService
    {
        Task<LotteryMerchanteOrder> FindOrderAsync(string id);

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="lvpOrderId">投注渠道订单号</param>
        /// <param name="lvpUserId">投注渠道用户编号</param>
        /// <param name="lvpVenderId">投注渠道编号</param>
        /// <param name="lotteryId">彩种</param>
        /// <param name="lotteryPlayId">玩法</param>
        /// <param name="issueNumber">期号</param>
        /// <param name="investCode">投注码</param>
        /// <param name="investType">投注类型</param>
        /// <param name="investCount">注数</param>
        /// <param name="investTimes">倍数</param>
        /// <param name="investAmount">投注金额</param>
        /// <returns>订单</returns>
        Task<LotteryMerchanteOrder> CreateAsync(string lvpOrderId, long? lvpUserId, string lvpVenderId, int lotteryId, int lotteryPlayId, int? issueNumber, string investCode, bool investType, short investCount, byte investTimes, int investAmount);

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task UpdateAsync(LotteryMerchanteOrder order);

        /// <summary>
        /// 出票成功
        /// </summary>
        /// <param name="ldpOrderId">出票渠道订单号</param>
        /// <param name="ldpVenderId">出票渠道编号</param>
        /// <param name="ticketedNumber">出票票号</param>
        /// <param name="ticketedTime">出票时间</param>
        /// <param name="ticketedOdds">出票赔率</param>
        /// <returns></returns>
        Task<LotteryMerchanteOrder> TicketedAsync(long ldpOrderId, string ldpVenderId, string ticketedNumber, DateTime ticketedTime, string ticketedOdds = null);

        /// <summary>
        /// 投注失败
        /// </summary>
        /// <param name="ldpOrderId">出票渠道订单号</param>
        /// <returns></returns>
        Task<LotteryMerchanteOrder> RejectedAsync(long ldpOrderId);

        /// <summary>
        /// 中间
        /// </summary>
        /// <param name="ldpOrderId">出票渠道订单号</param>
        /// <param name="amount">中军金额</param>
        /// <param name="aftertaxBonusAmount">税后中奖金额</param>
        /// <returns></returns>
        Task<LotteryMerchanteOrder> WinningAsync(long ldpOrderId, int amount, int aftertaxBonusAmount);

        /// <summary>
        /// 未中奖
        /// </summary>
        /// <param name="ldpOrderId">出票渠道订单号</param>
        /// <returns></returns>
        Task<LotteryMerchanteOrder> LoseingAsync(long ldpOrderId);
    }
}
