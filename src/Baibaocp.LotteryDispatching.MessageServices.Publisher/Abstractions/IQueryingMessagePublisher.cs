using Baibaocp.LotteryDispatching.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IQueryingMessagePublisher
    {

        Task PublishAsync(string merchanerId, string ldpOrderId, QueryingTypes queryingType);
    }
}
