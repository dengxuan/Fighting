using Baibaocp.LotteryDispatcher.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    [TransientDependency]
    public interface IExecuteHandler<in TExecuter> where TExecuter : IExecuter
    {

        Task<MessageHandle> HandleAsync(TExecuter executer);
    }
}