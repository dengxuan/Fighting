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
    internal class LotteryTicketedSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;

        private readonly ILotteryNoticingMessagePublisher _ticketingNoticeMessageService;

        private readonly ITicketingNotifier _dispatcher;

        private readonly ILogger<LotteryTicketedSubscriber> _logger;

        public LotteryTicketedSubscriber(IBusClient busClient, ITicketingNotifier dispatcher, ILotteryNoticingMessagePublisher ticketingNoticeMessageService, ILogger<LotteryTicketedSubscriber> logger)
        {
            _busClient = busClient;
            _ticketingNoticeMessageService = ticketingNoticeMessageService;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<NoticeMessage<LotteryTicketed>>(async (message) =>
            {
                try
                {
                    _logger.LogInformation("Received Ticketed LdpOrderId:{0} LdpVenderId:{1} Content:{2}", message.LdpOrderId, message.LdpMerchanerId, message.Content);
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
                        queue.WithName("LotteryNoticing.Tickets")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("#.Ticketed.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
