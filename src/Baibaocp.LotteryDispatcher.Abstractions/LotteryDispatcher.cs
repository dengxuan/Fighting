using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching
{
    public abstract class LotteryDispatcher<TExecuteMessage> : IExecuteDispatcher<TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {

        private readonly ILogger<LotteryDispatcher<TExecuteMessage>> _logger;

        public LotteryDispatcher(ILogger<LotteryDispatcher<TExecuteMessage>> logger)
        {
            _logger = logger;
        }

        public abstract Task<IExecuteHandle> DispatchAsync(TExecuteMessage message);
    }
}
