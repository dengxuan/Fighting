using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryAwardCalculator.Internal
{
    internal class LotteryAwarderService : BackgroundService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<LotteryAwarderService> _logger;

        private readonly CalculateHandler _handler;

        public LotteryAwarderService(IBusClient busClient, CalculateHandler handler, ILogger<LotteryAwarderService> logger)
        {
            _busClient = busClient;
            _handler = handler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _busClient.SubscribeAsync<LdpTicketedMessage>(async (message) =>
            {
                try
                {
                    _handler.Handle(message);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "算奖异常");
                }
                return new Nack();
            },
            context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryVender")
                                .WithType(ExchangeType.Topic)
                                .WithAutoDelete(false);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName("Awards.Calculating")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Tickets.Completed.Success");
                    });
                });
            }, stoppingToken);
        }
    }
}
