using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IDispatchOrderingMessageService
    {
        Task PublishAsync(long ldpOrderId, string ldpMerchanerId, LvpOrderMessage message);
    }
}
