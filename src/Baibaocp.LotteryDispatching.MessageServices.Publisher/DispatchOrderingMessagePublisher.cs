using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public class DispatchOrderingMessagePublisher : IDispatchOrderingMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<DispatchOrderingMessagePublisher> _logger;

        public DispatchOrderingMessagePublisher(IBusClient busClient, ILogger<DispatchOrderingMessagePublisher> logger)
        {
            _logger = logger;
            _busClient = busClient;
        }

        public Task PublishAsync(long ldpOrderId, string ldpMerchanerId,LvpOrderMessage message)
        {
            OrderingDispatchMessage orderingMessage = new OrderingDispatchMessage(ldpOrderId, ldpMerchanerId, message);

            return _busClient.PublishAsync(orderingMessage, context =>
            {
                context.UsePublishAcknowledge(false);
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryDispatching")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryDispatching.Ordering.{ldpMerchanerId}");
                });
            });
        }
    }
}
