using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IOrderingMessagePublisher
    {
        Task PublishAsync(string merchanerId, string ldpOrderId, LvpOrderedMessage message);
    }
}
