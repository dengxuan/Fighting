using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Fighting.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public class LotteryAwardingMessageService : ILotteryDispatcherMessageService<AwardingMessage>
    {
        private readonly IBusClient _busClient;

        private readonly ISchedulerManager _schedulerManager;

        private readonly ILogger<LotteryAwardingMessageService> _logger;

        private readonly IExecuteDispatcher<AwardingMessage> _dispatcher;

        public Task PublishAsync(string merchanerId, AwardingMessage executer)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<AwardingMessage>(async (executer) =>
            {
                try
                {

                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                    HandleTypes handle = await _dispatcher.DispatchAsync(executer);
                    if (handle == HandleTypes.Winning)
                    {
                        /* 中奖 */
                    }
                    else if (handle == HandleTypes.Loseing)
                    {
                        /* 未中奖 */
                    }
                    else if (handle == HandleTypes.Waiting)
                    {
                        return new Nack();
                    }
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering executer:{0} VenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                }
                return new Nack();
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
                        queue.WithName($"Orders.Dispatcher.{merchanerId}")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Orders.Storaged.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
