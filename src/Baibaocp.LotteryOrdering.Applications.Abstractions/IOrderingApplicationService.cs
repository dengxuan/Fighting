using Baibaocp.LotteryOrdering.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.ApplicationServices
{
    public interface IOrderingApplicationService
    {
        Task CreateAsync(LvpOrderMessage message);

        Task UpdateAsync(TicketedMessage message);

        Task UpdateAsync(AwardedMessage message);
    }
}
