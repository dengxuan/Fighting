using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IDispatchOrderingMessageService
    {
        Task PublishAsync(string ldpOrderId, string ldpMerchanerId, LvpOrderMessage message);
    }
}
