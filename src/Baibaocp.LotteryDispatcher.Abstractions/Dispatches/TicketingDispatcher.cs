using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.MessageServices;
using Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Dispatches
{
    public class TicketingDispatcher : IExecuterDispatcher<TicketingExecuteMessage>
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

        protected async Task TicketSuccess(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder, IDictionary<string, object> ticketContext)
        {
            /* 出票成功 */
            var message = new LdpTicketedMessage
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

        protected async Task TicketFailure(string ldpOrderId, string ldpVenderId, LvpOrderedMessage lvpOrder)
        {
            LdpTicketedMessage message = new LdpTicketedMessage
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

        public async Task<MessageHandle> DispatchAsync(TicketingExecuteMessage executer)
        {
            var handlerType = _options.GetHandler<TicketingExecuteMessage>(executer.LdpVenderId);
            var handler = (IExecuteHandler<TicketingExecuteMessage>)_resolver.GetRequiredService(handlerType);
            var handle = await handler.HandleAsync(executer);
            return handle;
            //using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    if (handle == MessageHandle.Success)
            //    {
            //        /* 出票成功 */
            //        await TicketSuccess(executer.LdpOrderId, executer.LdpVenderId, executer.LvpOrder, executer.TicketContext);
            //    }
            //    else if (handle == MessageHandle.Failure)
            //    {
            //        /* 出票失败 */
            //        await TicketFailure(executer.LdpOrderId, executer.LdpVenderId, executer.LvpOrder);
            //    }
            //    else if (handle == MessageHandle.Waiting)
            //    {
            //        BackgroundJob.Schedule<IExecuterDispatcher<TicketingExecuteMessage>>(dispatcher => dispatcher.DispatchAsync(executer), TimeSpan.FromSeconds(10));
            //    }
            //    transaction.Complete();
            //}
        }
    }
}
