using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    [TransientDependency]
    public interface IExecuteHandler<in TExecuteMessage>
    {

        Task<IHandle> HandleAsync(TExecuteMessage message);
    }
}