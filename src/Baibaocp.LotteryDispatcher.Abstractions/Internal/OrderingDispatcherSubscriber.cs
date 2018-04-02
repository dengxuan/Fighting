using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Internal
{
    internal class OrderingDispatcherSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly DispatcherConfiguration _dispatcherConfiguration;
        private readonly ILogger<OrderingDispatcherSubscriber> _logger;
        private readonly IOrderingDispatcher _orderingDispatcher;
        private readonly IDispatchOrderingMessageService _dispatchOrderingMessageService;
        private readonly IDispatchQueryingMessageService _dispatchQueryingMessageService;
        private readonly ILotteryNoticingMessagePublisher _lotteryNoticingMessagePublisher;

        public OrderingDispatcherSubscriber(IBusClient busClient, DispatcherConfiguration dispatcherOptions, ILogger<OrderingDispatcherSubscriber> logger, IOrderingDispatcher orderingDispatcher, IDispatchOrderingMessageService dispatchOrderingMessageService, ILotteryNoticingMessagePublisher lotteryNoticingMessagePublisher, IDispatchQueryingMessageService dispatchQueryingMessageService)
        {
            _logger = logger;
            _busClient = busClient;
            _dispatcherConfiguration = dispatcherOptions;
            _orderingDispatcher = orderingDispatcher;
            _dispatchOrderingMessageService = dispatchOrderingMessageService;
            _dispatchQueryingMessageService = dispatchQueryingMessageService;
            _lotteryNoticingMessagePublisher = lotteryNoticingMessagePublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _busClient.SubscribeAsync<OrderingDispatchMessage>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Received ordering message. LdpOrderId:{0} LvpMerchanerId:{1}", message.LdpOrderId, message.LdpMerchanerId);
                    var handle = await _orderingDispatcher.DispatchAsync(message);
                    switch (handle)
                    {
                        case AcceptedHandle accepted:
                            {
                                _logger.LogInformation($"Ordering Success: {message.LdpOrderId}-{message.LdpMerchanerId}-{message.LvpOrder.LvpOrderId}-{message.LvpOrder.LvpVenderId}-{message.LvpOrder.LotteryId}-{message.LvpOrder.LotteryPlayId}-{message.LvpOrder.InvestCode}");
                                await _dispatchQueryingMessageService.PublishAsync(message.LdpOrderId, message.LdpMerchanerId, message.LvpOrder.LvpOrderId, message.LvpOrder.LvpVenderId, message.LvpOrder.LotteryId, QueryingTypes.Ticketing);
                                return new Ack();
                            }
                        case RejectedHandle rejected:
                            {
                                if (rejected.Reorder)
                                {
                                    return new Nack();
                                }
                                _logger.LogInformation($"Ordering Rejected: {message.LdpMerchanerId}-{message.LvpOrder.LvpOrderId}-{message.LdpOrderId}");
                                await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{message.LdpMerchanerId}", new NoticeMessage<LotteryTicketed>(message.LdpOrderId, message.LdpMerchanerId, new LotteryTicketed
                                {
                                    LvpOrderId = message.LvpOrder.LvpOrderId,
                                    LvpMerchanerId = message.LvpOrder.LvpVenderId,
                                    TicketingType = LotteryTicketingTypes.Failure
                                }));
                                return new Ack();
                            }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering message processing. LdpOrderId:{0} LdpMerchanerId:{1}", message.LdpOrderId, message.LdpMerchanerId);
                }
                return new Nack();
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryDispatching")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName($"LotteryDispatching.{_dispatcherConfiguration.MerchanterName}.Orders")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey($"LotteryDispatching.Ordering.{_dispatcherConfiguration.MerchanterId}");
                    });
                });
            }, stoppingToken);
        }
    }
}
