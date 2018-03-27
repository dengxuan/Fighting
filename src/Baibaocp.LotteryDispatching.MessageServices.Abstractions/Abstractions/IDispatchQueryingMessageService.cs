using Baibaocp.LotteryDispatching.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IDispatchQueryingMessageService
    {

        Task PublishAsync(string ldpOrderId, string ldpMerchanerId,string lvpOrderId, string lvpMerchanerId, int lotteryId, QueryingTypes queryingType);
    }
}
