using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions.Abstractions
{
    public interface IAwardingNotifier
    {
        Task<bool> DispatchAsync(LvpAwardedMessage message);
    }
}
