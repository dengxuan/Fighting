using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Internal.Services
{
    internal class LotteryAwardedSubscriber : BackgroundService
    {

        private readonly IBusClient _busClient;

        private readonly IAwardingNotifier _dispatcher;

        private readonly ILogger<LotteryAwardedSubscriber> _logger;

        private readonly ILotteryNoticingMessagePublisher _awardingNoticeMessageService;

        public LotteryAwardedSubscriber(IBusClient busClient, IAwardingNotifier dispatcher, ILogger<LotteryAwardedSubscriber> logger, ILotteryNoticingMessagePublisher awardingNoticeMessageService)
        {
            _logger = logger;
            _busClient = busClient;
            _dispatcher = dispatcher;
            _awardingNoticeMessageService = awardingNoticeMessageService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<NoticeMessage<LotteryAwarded>>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Received ordering LvpOrderId:{0} LvpVenderId:{1}", message.LdpOrderId, message.LdpMerchanerId);
                    var result = await _dispatcher.DispatchAsync(message.Content);
                    if (result == true)
                    {
                        return new Ack();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering :{0}", message.LdpOrderId);
                }
                return new Nack();
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryNoticing")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName("LotteryNoticing.Awards")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("#.Awarded.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
