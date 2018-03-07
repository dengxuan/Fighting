using Baibaocp.LotteryDispatcher.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    [TransientDependency]
    public interface IExecuterDispatcher<TExecuter> where TExecuter : IExecuter
    {
        Task<MessageHandle> DispatchAsync(TExecuter executer);
    }
}
