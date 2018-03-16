using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface ITicketingNotifier
    {
        Task<bool> DispatchAsync(LvpTicketedMessage message);
    }
}
