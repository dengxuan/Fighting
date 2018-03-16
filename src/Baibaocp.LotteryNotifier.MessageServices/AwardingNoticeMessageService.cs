using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
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

        public Task SubscribeAsync(Func<LvpTicketedMessage, Task<bool>> subscriber, CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
