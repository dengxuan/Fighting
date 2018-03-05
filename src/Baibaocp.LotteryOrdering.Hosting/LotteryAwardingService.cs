using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Hosting
{
    public class LotteryAwardingService : BackgroundService
    {
        private readonly IBusClient _client;
        private readonly ILogger<LotteryAwardingService> _logger;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly IOrderingApplicationService _awardingApplicationService;

        public LotteryAwardingService(IBusClient client, IIdentityGenerater identityGenerater, ILogger<LotteryAwardingService> logger, IOrderingApplicationService awardingApplicationService)
        {
            _client = client;
            _logger = logger;
            _identityGenerater = identityGenerater;
            _awardingApplicationService = awardingApplicationService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.SubscribeAsync<LdpAwardedMessage>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Awarding received message:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
                    //await _awardingApplicationService.UpdateAsync(message);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Awarding received message error:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
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
                        queue.WithName("Awards.Storaging")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Awards.Completed.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
