using Baibaocp.LotteryDispatcher.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public interface IExecuterDispatcher<TExecuter> : ITransientDependency where TExecuter : IExecuter
    {
        Task<MessageHandle> DispatchAsync(TExecuter executer);
    }
}
