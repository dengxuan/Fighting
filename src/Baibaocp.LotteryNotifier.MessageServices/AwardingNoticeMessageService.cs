using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Notices;
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
    public class AwardingNoticeMessageService : IAwardingNoticeMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<AwardingNoticeMessageService> _logger;

        public AwardingNoticeMessageService(IBusClient busClient, ILogger<AwardingNoticeMessageService> logger)
        {
            _busClient = busClient;
            _logger = logger;
        }

        public Task PublishAsync(LvpAwardedMessage message)
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
                    configuration.WithRoutingKey($"LotteryNotifier.Awarding.{message.VenderId}");
                });
            });
        }

        public Task SubscribeAsync(Func<Notice<Awarded>, Task<bool>> subscriber, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LvpAwardedMessage>(async (message) =>
            {
                try
                {
                    var notice = new Notice<Awarded>(message.VenderId)
                    {
                        Content = new Awarded
                        {
                            OrderId = message.OrderId,
                            Amount = message.AftertaxAmount,
                            Status = message.AwardingType == LotteryAwardingTypes.Winning ? 10400 : 10401
                        }
                    };
                    bool result = await subscriber?.Invoke(notice);
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
                        queue.WithName("LotteryOrdering.Awards")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Awarding.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
