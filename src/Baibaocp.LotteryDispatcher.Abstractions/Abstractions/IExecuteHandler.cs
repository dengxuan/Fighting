using Baibaocp.LotteryDispatching.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    [TransientDependency]
    public interface IExecuteHandler<in TExecuter> where TExecuter : IExecuter
    {

        Task<MessageHandle> HandleAsync(TExecuter executer);
    }
}