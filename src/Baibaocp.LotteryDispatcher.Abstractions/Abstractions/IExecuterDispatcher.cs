using Baibaocp.LotteryDispatcher.MessageServices;
using Baibaocp.LotteryDispatcher.MessageServices.Messages;
using Fighting.DependencyInjection.Builder;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Abstractions
{
    public interface IExecuterDispatcher<TExecuter> : ITransientDependency where TExecuter : ExecuteMessage
    {
        Task<MessageHandle> DispatchAsync(TExecuter executer);
    }
}
