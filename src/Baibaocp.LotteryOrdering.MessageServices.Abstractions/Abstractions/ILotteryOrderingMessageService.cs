using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.Abstractions
{
    public interface ILotteryOrderingMessageService : IMessageService
    {
        Task PublishAsync(LvpOrderMessage message);
    }
}
