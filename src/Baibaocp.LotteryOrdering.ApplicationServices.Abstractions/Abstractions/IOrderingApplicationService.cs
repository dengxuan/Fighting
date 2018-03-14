using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Fighting.ApplicationServices.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.ApplicationServices.Abstractions
{
    public interface IOrderingApplicationService : IApplicationService
    {
        Task<LotteryMerchanteOrder> FindOrderAsync(string id);

        Task CreateAsync(string lvpOrderId, long? lvpUserId, string lvpVenderId, int lotteryId, int lotteryPlayId, int? issueNumber, string investCode, bool investType, short investCount, byte investTimes, int investAmount);

        Task UpdateAsync(LotteryMerchanteOrder order);

        Task TicketedAsync(long lvpOrderId, string ldpOrderId, string ldpVenderId, string ticketOdds, int status);

        Task RewardedAsync(long lvpOrderId, int amount, int status);
    }
}
