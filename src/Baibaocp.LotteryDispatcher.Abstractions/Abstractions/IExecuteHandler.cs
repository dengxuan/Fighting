using Baibaocp.LotteryDispatcher.MessageServices;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{

    public interface IExecuteHandler<in TExecuter> : ITransientDependency where TExecuter : IExecuter
    {

        Task<MessageHandle> HandleAsync(TExecuter executer);
    }
}