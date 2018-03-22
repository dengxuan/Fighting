using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Abstractions;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class LotteryOrderingMessageService : ILotteryOrderingMessageService
    {
        private readonly IBusClient _busClient;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly ILogger<LotteryOrderingMessageService> _logger;
        private readonly IDispatchOrderingMessageService _dispatchOrderingMessageService;
        private readonly IOrderingApplicationService _orderingApplicationService;
        private readonly ILotteryMerchanterApplicationService _lotteryMerchanterApplicationService;

        public LotteryOrderingMessageService(IBusClient busClient, IIdentityGenerater identityGenerater, IOrderingApplicationService orderingApplicationService, ILotteryMerchanterApplicationService lotteryMerchanterApplicationService, ILogger<LotteryOrderingMessageService> logger, IDispatchOrderingMessageService dispatchOrderingMessageService)
        {
            _logger = logger;
            _busClient = busClient;
            _identityGenerater = identityGenerater;
            _orderingApplicationService = orderingApplicationService;
            _lotteryMerchanterApplicationService = lotteryMerchanterApplicationService;
            _dispatchOrderingMessageService = dispatchOrderingMessageService;
        }

        public Task PublishAsync(LvpOrderMessage orderingMessage)
        {
            return _busClient.PublishAsync(orderingMessage, context =>
            {
                context.UsePublishAcknowledge(false);
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryOrdering")
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryOrdering.Accepted.{orderingMessage.LvpVenderId}");
                });
            });
        }
    }
}
