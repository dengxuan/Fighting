using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Executers;
using Baibaocp.LotteryDispatcher.MessageServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Dispatches
{
    public class AwardingDispatcher : IExecuterDispatcher<AwardingExecuter>
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        public AwardingDispatcher(IServiceProvider resolver, ILogger<OrderingDispatcher> logger, LotteryDispatcherOptions options)
        {
            _logger = logger;
            _options = options;
            _resolver = resolver;
        }

        public async Task<MessageHandle> DispatchAsync(AwardingExecuter executer)
        {
            var handlerType = _options.GetHandler<AwardingExecuter>(executer.LdpVenderId);
            var handler = (IExecuteHandler<AwardingExecuter>)_resolver.GetRequiredService(handlerType);
            var handle = await handler.HandleAsync(executer);
            return handle;
        }
    }
}
