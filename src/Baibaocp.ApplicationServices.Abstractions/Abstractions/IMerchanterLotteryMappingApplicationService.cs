using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface IMerchanterLotteryMappingApplicationService
    {
        /// <summary>
        /// 根据投注渠道商编号和彩种查询出票渠道商编号
        /// </summary>
        /// <param name="lvpVenderId">投注渠道商编号</param>
        /// <param name="lotteryId">彩种编号</param>
        /// <returns></returns>
        Task<string> FindLdpVenderId(string lvpVenderId, int lotteryId);
    }
}
