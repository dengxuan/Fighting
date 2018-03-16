using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.MessageServices
{
    public class TicketingNoticeMessageService : ITicketingNoticeMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<TicketingNoticeMessageService> _logger;

        public TicketingNoticeMessageService(IBusClient busClient, ILogger<TicketingNoticeMessageService> logger)
        {
            _busClient = busClient;
            _logger = logger;
        }

        public Task PublishAsync(LvpTicketedMessage message)
        {
            return _busClient.PublishAsync(message, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryNotifier")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryNotifier.Ticketing.{message.VenderId}");
                });
            });
        }

        public Task SubscribeAsync(Func<LvpTicketedMessage, Task<bool>> subscriber, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LvpTicketedMessage>(async (message) =>
            {
                try
                {
                    bool result = await subscriber?.Invoke(message);
                    //bool result = await _dispatcher.DispatchAsync(new Notifier<Ticketed>(message.VenderId)
                    //{
                    //    Notice = new Ticketed
                    //    {
                    //        OrderId = message.OrderId,
                    //        TicketOdds = message.TicketOdds,
                    //        Status = message.TicketingType == LotteryTicketingTypes.Success ? 10300 : 10301
                    //    }
                    //});
                    if (result == true)
                    {
                        return new Ack();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering :{0}", message.OrderId);
                }
                return new Nack();
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryNotifier")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName("LotteryOrdering.Tickets")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Ticketing.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
