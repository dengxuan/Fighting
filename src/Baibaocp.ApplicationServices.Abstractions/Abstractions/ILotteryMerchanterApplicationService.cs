using Baibaocp.Storaging.Entities.Merchants;
using Fighting.ApplicationServices.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface ILotteryMerchanterApplicationService : IApplicationService
    {
        /// <summary>
        /// 查询渠道商
        /// </summary>
        /// <param name="merchanterId"></param>
        /// <returns></returns>
        Task<Merchanter> FindMerchanterAsync(string merchanterId);

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">充值金额</param>
        /// <returns></returns>
        Task Recharging(string merchanterId, string orderId, int amount);

        /// <summary>
        /// 出票
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="orderId">订单号</param>
        /// <param name="lotteryId">彩种编号</param>
        /// <param name="amount">出票金额</param>
        /// <returns></returns>
        Task Ticketing(string merchanterId, string orderId, int lotteryId, int amount);

        /// <summary>
        /// 返奖
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="orderId">订单号</param>
        /// <param name="lotteryId">彩种编号</param>
        /// <param name="amount">返奖金额</param>
        /// <returns></returns>
        Task Rewarding(string merchanterId, string orderId, int lotteryId, int amount);

        /// <summary>
        /// 根据投注渠道商编号和彩种查询出票渠道商编号
        /// </summary>
        /// <param name="lvpMerchanterId">投注渠道商编号</param>
        /// <param name="lotteryId">彩种编号</param>
        /// <returns></returns>
        Task<string> FindLdpMerchanterIdAsync(string lvpMerchanterId, int lotteryId);
    }
}
