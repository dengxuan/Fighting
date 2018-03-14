using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Internal
{
    internal class TicketingDispatcherMessageServer : BackgroundService
    {
        private readonly IServiceProvider _iocResolver;
        private readonly IQueryingMessageService _orderingMessageService;
        private readonly DispatcherConfiguration _dispatcherOptions;
        private readonly ILogger<OrderingDispatcherMessageServer> _logger;
        private readonly ITicketingExecuteDispatcher _dispatcher;

        public TicketingDispatcherMessageServer(DispatcherConfiguration dispatcherOptions, ILogger<OrderingDispatcherMessageServer> logger, IServiceProvider iocResolver)
        {
            _logger = logger;
            _dispatcherOptions = dispatcherOptions;
            _iocResolver = iocResolver;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _orderingMessageService.SubscribeAsync(_dispatcherOptions.MerchanterId, "", QueryingTypes.Ticketing, async (message) =>
            {
                await _dispatcher.DispatchAsync(message);
                return true;
            }, stoppingToken);
        }
    }
}
