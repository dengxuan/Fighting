using Baibaocp.LotteryDispatching.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    [TransientDependency]
    public interface IExecuterDispatcher<TExecuter> where TExecuter : IExecuter
    {
        Task<MessageHandle> DispatchAsync(TExecuter executer);
    }
}
