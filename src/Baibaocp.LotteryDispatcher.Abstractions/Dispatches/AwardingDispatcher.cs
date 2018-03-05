using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.MessageServices;
using Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Dispatches
{
    public class AwardingDispatcher : IExecuterDispatcher<AwardingExecuteMessage>
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

        [Queue("Awarding")]
        public async Task<MessageHandle> DispatchAsync(AwardingExecuteMessage executer)
        {
            var handlerType = _options.GetHandler<AwardingExecuteMessage>(executer.LdpVenderId);
            var handler = (IExecuteHandler<AwardingExecuteMessage>)_resolver.GetRequiredService(handlerType);
            var handle = await handler.HandleAsync(executer);
            return handle;
            //if(handle == MessageHandle.Waiting)
            //{
            //    BackgroundJob.Schedule<IExecuterDispatcher<AwardingExecuteMessage>>(dispatcher => dispatcher.DispatchAsync(executer), TimeSpan.FromSeconds(30));
            //}
        }
    }
}
