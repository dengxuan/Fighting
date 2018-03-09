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
            switch (handle.HandleType)
            {
                case HandleTypes.Accepted:
                    break;
                case HandleTypes.Rejected:
                    break;
                case HandleTypes.Success:
                    break;
                case HandleTypes.Failure:
                    break;
                case HandleTypes.Winning:
                    break;
                case HandleTypes.Loseing:
                    break;
                case HandleTypes.Waiting: return false;
            }
            return true;
        }
    }
}
