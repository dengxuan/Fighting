using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching
{
    public class LotteryDispatcher<TExecuteMessage> : IExecuteDispatcher<TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {
        private readonly ILogger<LotteryDispatcher<TExecuteMessage>> _logger;

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        public LotteryDispatcher(IServiceProvider resolver, ILogger<LotteryDispatcher<TExecuteMessage>> logger, LotteryDispatcherOptions options)
        {
            _logger = logger;
            _options = options;
            _resolver = resolver;
        }

        public async Task<bool> DispatchAsync(TExecuteMessage message)
        {
            var handlerType = _options.GetHandler<TExecuteMessage>(message.LdpVenderId);
            var handler = (IExecuteHandler<TExecuteMessage>)_resolver.GetRequiredService(handlerType);
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
