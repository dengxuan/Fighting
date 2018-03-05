using Baibaocp.LotteryDispatcher.MessageServices;
using Baibaocp.LotteryDispatcher.MessageServices.Messages;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{

    public interface IExecuteHandler<in TExecuter> : ITransientDependency where TExecuter : ExecuteMessage
    {

        Task<MessageHandle> HandleAsync(TExecuter executer);
    }
}