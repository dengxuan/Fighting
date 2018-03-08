using Baibaocp.LotteryDispatching.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    [TransientDependency]
    public interface IExecuteDispatcher<TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {
        Task<MessageHandle> DispatchAsync(TExecuteMessage message);
    }
}
