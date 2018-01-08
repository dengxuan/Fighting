using Baibaocp.LotteryDispatcher.Core;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public interface IExecuterDispatcher<TExecuter> : ITransientDependency where TExecuter : IExecuter
    {
        Task DispatchAsync(TExecuter executer);
    }
}
