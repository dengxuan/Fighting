using Baibaocp.LotteryNotifier.MessageServices;
using Baibaocp.LotteryNotifier.MessageServices.Notices;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface IAwardingNotifier
    {
        Task<bool> DispatchAsync(Notice<Awarded> message);
    }
}
