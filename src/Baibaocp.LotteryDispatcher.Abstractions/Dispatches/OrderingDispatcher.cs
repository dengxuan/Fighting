﻿using Baibaocp.Core;
using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Core.Executers;
using Baibaocp.LotteryOrdering.Messages;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.LotteryDispatcher.Dispatches
{
    public class OrderingDispatcher : IExecuterDispatcher<OrderingExecuter>
    {
        private readonly ILogger _logger;

        private readonly IBusClient _client;

        private readonly IServiceProvider _resolver;

        private readonly LotteryDispatcherOptions _options;

        public OrderingDispatcher(IServiceProvider resolver, ILogger<OrderingDispatcher> logger, LotteryDispatcherOptions options, IBusClient client)
        {
            _logger = logger;
            _client = client;
            _options = options;
            _resolver = resolver;
        }

        protected async Task OrderingFailure(string ldpOrderId, string ldpVenderId, LvpOrderMessage lvpOrder)
        {
            var message = new TicketedMessage
            {
                LdpOrderId = ldpOrderId,
                LdpVenderId = ldpVenderId,
                LvpOrder = lvpOrder,
                Status = OrderStatus.TicketNotRecv,
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
                    configuration.WithRoutingKey(RoutingkeyConsts.Orders.Completed.Failure);
                });
            });
        }

        [Queue("Ordering")]
        public async Task DispatchAsync(OrderingExecuter executer)
        {
            var handlerType = _options.GetHandler<OrderingExecuter>(executer.LdpVenderId);
            var handler = (IExecuteHandler<OrderingExecuter>)_resolver.GetRequiredService(handlerType);
            var handle = await handler.HandleAsync(executer);
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
            {
                if (handle == Handle.Accepted)
                {
                    /* 投注成功，添加出票查询计划任务 */

                    BackgroundJob.Schedule<IExecuterDispatcher<TicketingExecuter>>(dispatcher => dispatcher.DispatchAsync(new TicketingExecuter(executer.LdpOrderId, executer.LdpVenderId, executer.LvpOrder)), TimeSpan.FromSeconds(10));
                }
                else if (handle == Handle.Rejected)
                {
                    /* 投注失败，将数据存入队列，进行通知和*/
                    await OrderingFailure(executer.LdpOrderId, executer.LdpVenderId, executer.LvpOrder);
                }
                else if (handle == Handle.Waiting)
                {
                    BackgroundJob.Schedule<IExecuterDispatcher<OrderingExecuter>>(dispatcher => dispatcher.DispatchAsync(executer), TimeSpan.FromSeconds(10));
                }
                transaction.Complete();
            }
        }
    }
}
