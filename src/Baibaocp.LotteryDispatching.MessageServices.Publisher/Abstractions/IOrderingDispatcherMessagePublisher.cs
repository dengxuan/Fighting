using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IOrderingDispatcherMessagePublisher
    {
        Task PublishAsync(string merchanerId, string ldpOrderId, LvpOrderedMessage message);
    }
}
