using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Fighting.ApplicationServices.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.ApplicationServices.Abstractions
{
    public interface IOrderingApplicationService : IApplicationService
    {
        Task<LotteryMerchanteOrder> FindOrderAsync(string id);

        Task CreateAsync(LotteryMerchanteOrder message);

        Task UpdateAsync(LotteryMerchanteOrder message);

        Task TicketedAsync(long lvpOrderId, string ldpOrderId, string ticketOdds, int status);

        Task RewardedAsync(long lvpOrderId, int amount, int status);
    }
}
