using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.MessageServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher
{
    public class LotteryDispatcher<TExecuter> : IExecuterDispatcher<TExecuter> where TExecuter : IExecuter
    {
        private readonly ILogger<LotteryDispatcher<TExecuter>> _logger;

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        public LotteryDispatcher(IServiceProvider resolver, ILogger<LotteryDispatcher<TExecuter>> logger, LotteryDispatcherOptions options)
        {
            _logger = logger;
            _options = options;
            _resolver = resolver;
        }

        public async Task<MessageHandle> DispatchAsync(TExecuter executer)
        {
            var handlerType = _options.GetHandler<TExecuter>(executer.LdpVenderId);
            var handler = (IExecuteHandler<TExecuter>)_resolver.GetRequiredService(handlerType);
            var handle = await handler.HandleAsync(executer);
            return handle;
        }
    }
}
