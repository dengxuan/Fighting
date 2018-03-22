using Baibaocp.LotteryNotifier.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface IAwardingNotifier
    {
        Task<bool> DispatchAsync(LotteryAwarded message);
    }
}
