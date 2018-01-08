using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Core.Executers;
using Baibaocp.LotteryOrdering.Messages;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.LotteryDispatcher.Dispatches
{
    public class TicketingDispatcher : IExecuterDispatcher<TicketingExecuter>
    {

        private readonly IServiceProvider _resolver;

        private readonly IBusClient _client;

        private readonly LotteryDispatcherOptions _options;

        private readonly ILogger<OrderingDispatcher> _logger;

        public TicketingDispatcher(IServiceProvider resolver, LotteryDispatcherOptions options, ILogger<OrderingDispatcher> logger, IBusClient client)
        {
            _logger = logger;
            _client = client;
            _options = options;
            _resolver = resolver;
        }

        protected async Task TicketSuccess(string ldpOrderId, string ldpVenderId, LvpOrderMessage lvpOrder, IDictionary<string, object> ticketContext)
        {
            /* 出票成功 */
            var message = new TicketedMessage
            {
                LdpOrderId = ldpOrderId,
                LdpVenderId = ldpVenderId,
                LvpOrder = lvpOrder,
                TicketOdds = ticketContext["TicketOdds"] as string,
                Status = OrderStatus.TicketDrawing,
            };
            await _client.PublishAsync(message, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryVender")
                                .WithType(ExchangeType.Topic)
                                .WithDurability(true)
                                .WithAutoDelete(false);
                    });
                    configuration.WithRoutingKey(RoutingkeyConsts.Tickets.Completed.Success);
                });
            });
        }

        protected async Task TicketFailure(string ldpOrderId, string ldpVenderId, LvpOrderMessage lvpOrder)
        {
            TicketedMessage message = new TicketedMessage
            {
                LdpOrderId = ldpOrderId,
                LdpVenderId = ldpVenderId,
                LvpOrder = lvpOrder,
                Status = OrderStatus.TicketNotRecv
            };

            await _client.PublishAsync(message, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryVender")
                                .WithType(ExchangeType.Topic)
                                .WithDurability(true)
                                .WithAutoDelete(false);
                    });
                    configuration.WithRoutingKey(RoutingkeyConsts.Tickets.Completed.Failure);
                });
            });
        }


        [Queue("Ticketing")]
        public async Task DispatchAsync(TicketingExecuter executer)
        {
            var handlerType = _options.GetHandler<TicketingExecuter>(executer.LdpVenderId);
            var handler = (IExecuteHandler<TicketingExecuter>)_resolver.GetRequiredService(handlerType);
            var handle = await handler.HandleAsync(executer);
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
            {
                if (handle == Handle.Success)
                {
                    /* 出票成功 */
                    await TicketSuccess(executer.LdpOrderId, executer.LdpVenderId, executer.LvpOrder, executer.TicketContext);
                }
                else if (handle == Handle.Failure)
                {
                    /* 出票失败 */
                    await TicketFailure(executer.LdpOrderId, executer.LdpVenderId, executer.LvpOrder);
                }
                else if (handle == Handle.Waiting)
                {
                    BackgroundJob.Schedule<IExecuterDispatcher<TicketingExecuter>>(dispatcher => dispatcher.DispatchAsync(executer), TimeSpan.FromSeconds(10));
                }
                transaction.Complete();
            }
        }
    }
}
