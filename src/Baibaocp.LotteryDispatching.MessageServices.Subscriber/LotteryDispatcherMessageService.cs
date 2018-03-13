using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace Baibaocp.LotteryDispatching
{
    public class LotteryDispatcherMessageService : BackgroundService
    {
        private readonly IServiceProvider _iocResolver;
        private readonly DispatcherConfiguration _dispatcherOptions;
        private readonly ILogger<LotteryDispatcherMessageService> _logger;

        public LotteryDispatcherMessageService(DispatcherConfiguration dispatcherOptions, ILogger<LotteryDispatcherMessageService> logger, IServiceProvider iocResolver)
        {
            _logger = logger;
            _dispatcherOptions = dispatcherOptions;
            _iocResolver = iocResolver;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IEnumerable<ILotteryDispatcherMessageSubscriber> messageServices = _iocResolver.GetServices<ILotteryDispatcherMessageSubscriber>();
            foreach (var messageService in messageServices)
            {
               await messageService.SubscribeAsync(_dispatcherOptions.MerchanterId, stoppingToken);
            }
        }
    }
}
