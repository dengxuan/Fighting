using Baibaocp.LotteryOrdering.Core.Entities;
using Baibaocp.LotteryOrdering.Messages;
using Fighting.ApplicationServices.Abstractions;
using Orleans;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.ApplicationServices
{
    public interface IOrderingApplicationService : IApplicationService
    {
        Task<LotteryVenderOrderEntity> FindOrderAsync(string id);

        Task CreateAsync(LvpOrderMessage message);

        Task UpdateAsync(TicketedMessage message);

        Task UpdateAsync(AwardedMessage message);
    }
}
