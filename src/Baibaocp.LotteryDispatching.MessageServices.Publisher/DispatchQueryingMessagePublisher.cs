using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public class DispatchQueryingMessagePublisher : IDispatchQueryingMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<DispatchQueryingMessagePublisher> _logger;

        public DispatchQueryingMessagePublisher(IBusClient busClient, ILogger<DispatchQueryingMessagePublisher> logger)
        {
            _logger = logger;
            _busClient = busClient;
        }

        public Task PublishAsync(long ldpOrderId, string ldpMerchanerId, string lvpOrderId, string lvpMerchanerId,QueryingTypes queryingType)
        {
            QueryingExecuteMessage queryingMessage = new QueryingExecuteMessage(ldpOrderId, ldpMerchanerId, lvpOrderId, lvpMerchanerId, queryingType);
            return _busClient.PublishAsync(queryingMessage, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryDispatching")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryDispatching.Querying.{ldpMerchanerId}");
                });
            });
        }
    }
}
