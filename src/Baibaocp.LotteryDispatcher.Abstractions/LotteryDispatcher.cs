using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching
{
    internal class LotteryDispatcher<TExecuteMessage> : IExecuteDispatcher<TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {

        private readonly IServiceProvider _resolver;

        private readonly ILogger<LotteryDispatcher<TExecuteMessage>> _logger;

        public LotteryDispatcher(IServiceProvider resolver, ILogger<LotteryDispatcher<TExecuteMessage>> logger)
        {
            _logger = logger;
            _resolver = resolver;
        }

        public async Task<bool> DispatchAsync(TExecuteMessage message)
        {
            var handler = _resolver.GetRequiredService<IExecuteHandler<TExecuteMessage>>();
            var handle = await handler.HandleAsync(message);
            switch (handle)
            {
                case Accepted accepted:
                    break;
                case Rejected rejected:
                    break;
                case Success success:
                    break;
                case Failure failure:
                    break;
                case Winning winning:
                    break;
                case Loseing loseing:
                    break;
                case Waiting waiting: return false;
            }
            return true;
        }
    }
}
