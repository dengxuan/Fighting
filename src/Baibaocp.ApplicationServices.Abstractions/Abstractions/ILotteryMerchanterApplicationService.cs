using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface ILotteryMerchanterApplicationService
    {

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">充值金额</param>
        /// <returns></returns>
        Task Recharging(int merchanterId, long orderId, int amount);

        /// <summary>
        /// 出票
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">出票金额</param>
        /// <returns></returns>
        Task Ticketing(int merchanterId, long orderId, int amount);

        /// <summary>
        /// 返奖
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">返奖金额</param>
        /// <returns></returns>
        Task Rewarding(int merchanterId, long orderId, int amount);
    }
}
