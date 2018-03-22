using Baibaocp.LotteryNotifier.MessageServices;
using Baibaocp.LotteryNotifier.MessageServices.Notices;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface ITicketingNotifier
    {
        Task<bool> DispatchAsync(Notice<Ticketed> notice);
    }
}
