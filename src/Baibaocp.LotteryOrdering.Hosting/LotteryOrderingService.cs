using Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities;
using Fighting.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.LotteryOrdering.Hosting
{
    public class LotteryOrderingService : BackgroundService
    {
        private readonly IBusClient _client;
        private readonly ICacheManager _cacheManager;
        private readonly HostingConfugiration _options;
        private readonly ILogger<LotteryOrderingService> _logger;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly IOrderingApplicationService _orderingApplicationService;

        public LotteryOrderingService(IBusClient client, ICacheManager cacheManager, HostingConfugiration options, IIdentityGenerater identityGenerater, ILogger<LotteryOrderingService> logger, IOrderingApplicationService orderingApplicationService)
        {
            _client = client;
            _cacheManager = cacheManager;
            _options = options;
            _logger = logger;
            _identityGenerater = identityGenerater;
            _orderingApplicationService = orderingApplicationService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.SubscribeAsync<LvpOrderedMessage>(async (message) =>
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
                    {
                        _logger.LogTrace("Ordering received message:{0} LvpVenderId:{1}", message.LvpOrderId, message.LvpVenderId);
                        //await _orderingApplicationService.CreateAsync(message);
                        OrderingExecuteMessage executer = new OrderingExecuteMessage(_identityGenerater.Generate().ToString(), _options.LdpVenderId, message);
                        _logger.LogTrace("Publish executer message:{0} LdpVenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                        await _client.PublishAsync(executer, context =>
                        {
                            context.UsePublishConfiguration(configuration =>
                            {
                                configuration.OnDeclaredExchange(exchange =>
                                {
                                    exchange.WithName("Baibaocp.LotteryVender")
                                            .WithDurability(true)
                                            .WithAutoDelete(false)
                                            .WithType(ExchangeType.Topic);
                                });
                                configuration.WithRoutingKey(RoutingkeyConsts.Orders.Storaged);
                            });
                        });
                        transaction.Complete();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Ordering received message error:{0} LvpVenderId:{1} Exception:{2}", message.LvpOrderId, message.LvpVenderId, ex);
                }

                /* 投注失败 */
                await _client.PublishAsync(new LdpTicketedMessage
                {
                    LvpOrder = message,
                    Status = OrderStatus.TicketFailed
                }, context => context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryVender")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey(RoutingkeyConsts.Orders.Completed.Failure);
                }));
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryVender")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName("Orders.Storaging")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Orders.Accepted.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
