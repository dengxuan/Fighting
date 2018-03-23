using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
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
    internal class QueryingDispatcherSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly IQueryingDispatcher _queryingDispatcher;
        private readonly ILogger<OrderingDispatcherSubscriber> _logger;
        private readonly DispatcherConfiguration _dispatcherConfiguration;
        private readonly IDispatchQueryingMessageService _dispatchQueryingMessageService;
        private readonly ILotteryNoticingMessagePublisher _lotteryNoticingMessagePublisher;

        public QueryingDispatcherSubscriber(IBusClient busClient, DispatcherConfiguration dispatcherConfiguration, ILogger<OrderingDispatcherSubscriber> logger, IQueryingDispatcher queryingDispatcher, IDispatchQueryingMessageService dispatchQueryingMessageService, ILotteryNoticingMessagePublisher lotteryNoticingMessagePublisher)
        {
            _logger = logger;
            _busClient = busClient;
            _dispatcherConfiguration = dispatcherConfiguration;
            _queryingDispatcher = queryingDispatcher;
            _dispatchQueryingMessageService = dispatchQueryingMessageService;
            _lotteryNoticingMessagePublisher = lotteryNoticingMessagePublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _busClient.SubscribeAsync<QueryingDispatchMessage>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Received querying {0} message. LdpOrderId:{1} LdpMerchanerId:{2}", message.QueryingType, message.LdpOrderId, message.LdpMerchanerId);
                    var handle = await _queryingDispatcher.DispatchAsync(message);
                    switch (handle)
                    {
                        case SuccessHandle success:
                            {
                                _logger.LogInformation($"Ticket Success: {message.LdpOrderId}-{message.LdpMerchanerId}-{message.LvpOrderId}-{message.LvpMerchanerId}");
                                await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{message.LdpMerchanerId}", new NoticeMessage<LotteryTicketed>(message.LdpOrderId, message.LdpMerchanerId, new LotteryTicketed
                                {
                                    LvpMerchanerId = message.LvpMerchanerId,
                                    LvpOrderId = message.LvpOrderId,
                                    TicketingType = LotteryTicketingTypes.Success,
                                    TicketOdds = success.TicketOdds,
                                }));
                                return new Ack();
                            }
                        case FailureHandle failure:
                            {
                                _logger.LogInformation($"Ticket Failure: {message.LdpOrderId}-{message.LdpMerchanerId}-{message.LvpOrderId}-{message.LvpMerchanerId}");
                                await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{message.LdpMerchanerId}", new NoticeMessage<LotteryTicketed>(message.LdpOrderId, message.LdpMerchanerId, new LotteryTicketed
                                {
                                    LvpMerchanerId = message.LvpMerchanerId,
                                    LvpOrderId = message.LvpOrderId,
                                    TicketingType = LotteryTicketingTypes.Failure,
                                }));
                                return new Ack();
                            }
                        case WinningHandle winning:
                            {
                                return new Ack();
                            }
                        case LoseingHandle loseing:
                            {
                                return new Ack();
                            }
                        case WaitingHandle waiting:
                            {
                                return new Nack();
                            }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the querying {0} message. LdpOrderId:{1} LdpMerchanerId:{2}", message.QueryingType, message.LdpOrderId, message.LdpMerchanerId);
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
                        queue.WithName($"LotteryDispatching.{_dispatcherConfiguration.MerchanterName}.Queries")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey($"LotteryDispatching.Querying.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
