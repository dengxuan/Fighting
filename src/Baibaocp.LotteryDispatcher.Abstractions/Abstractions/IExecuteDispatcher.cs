using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    [TransientDependency]
    public interface IExecuteDispatcher<in TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {
        Task<bool> DispatchAsync(TExecuteMessage message);
    }
}
