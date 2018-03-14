using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public class QueryingMessagePublisher : IQueryingDispatcherMessagePublisher
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<QueryingMessagePublisher> _logger;

        public QueryingMessagePublisher(IBusClient busClient, ILogger<QueryingMessagePublisher> logger)
        {
            _logger = logger;
            _busClient = busClient;
        }

        public Task PublishAsync(string merchanerId, string ldpOrderId, QueryingTypes queryingType)
        {
            QueryingExecuteMessage queryingMessage = new QueryingExecuteMessage(ldpOrderId, merchanerId);
            return _busClient.PublishAsync(queryingMessage, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryDispatcher")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryDispatcher.{queryingType}.{merchanerId}");
                });
            });
        }
    }
}
