using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.MessageServices
{
    public class LotteryNoticingMessageService : ILotteryNoticingMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<LotteryNoticingMessageService> _logger;

        public LotteryNoticingMessageService(IBusClient busClient, ILogger<LotteryNoticingMessageService> logger)
        {
            _busClient = busClient;
            _logger = logger;
        }

        public Task PublishAsync<TContent>(string routingKey, NoticeMessage<TContent> message) where TContent : class, INoticeContent
        {
            return _busClient.PublishAsync(message, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryNoticing")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey(routingKey);
                });
            });
        }
    }
}
