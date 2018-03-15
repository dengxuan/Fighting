using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Internal
{
    internal class AwardingDispatcherService : BackgroundService
    {
        private readonly DispatcherConfiguration _dispatcherConfiguration;
        private readonly ILogger<OrderingDispatcherService> _logger;
        private readonly IAwardingDispatcher _awardingDispatcher;
        private readonly IQueryingMessageService _orderingMessageService;

        public AwardingDispatcherService(DispatcherConfiguration dispatcherConfiguration, ILogger<OrderingDispatcherService> logger, IAwardingDispatcher awardingDispatcher, IQueryingMessageService queryingMessageService)
        {
            _logger = logger;
            _dispatcherConfiguration = dispatcherConfiguration;
            _awardingDispatcher = awardingDispatcher;
            _orderingMessageService = queryingMessageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _orderingMessageService.SubscribeAsync(_dispatcherConfiguration.MerchanterName, QueryingTypes.Awarding, async (message) =>
             {
                 var handle = await _awardingDispatcher.DispatchAsync(message);
                 switch (handle)
                 {
                     case WinningHandle winning:
                         {
                             return true;
                         }
                     case LoseingHandle loseing:
                         {
                             return true;
                         }
                     case WaitingHandle waiting:
                         {
                             return false;
                         }
                     default:
                         {
                             return true;
                         }
                 }
             }, stoppingToken);
        }
    }
}
