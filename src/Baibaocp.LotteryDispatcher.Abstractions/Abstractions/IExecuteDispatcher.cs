using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    [TransientDependency]
    public interface IExecuteDispatcher<in TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {
        string Name { get; }

        Task<IExecuteHandle> DispatchAsync(TExecuteMessage message);
    }
}
