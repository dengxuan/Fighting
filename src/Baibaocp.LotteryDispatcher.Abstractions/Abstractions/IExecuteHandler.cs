using Baibaocp.LotteryDispatching.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    [TransientDependency]
    public interface IExecuteHandler<in TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {

        Task<IHandle> HandleAsync(TExecuteMessage message);
    }
}