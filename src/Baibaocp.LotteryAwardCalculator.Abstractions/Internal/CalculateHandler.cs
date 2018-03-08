using Baibaocp.LotteryAwardCalculator.Abstractions;
using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Executers;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System;

namespace Baibaocp.LotteryAwardCalculator.Internal
{
    public class CalculateHandler
    {
        private readonly IBusClient _busClient;

        private readonly IBackgroundJobClient _jobClient;

        private readonly ICalculator _caculator;

        private readonly ILogger<CalculateHandler> _logger;

        public CalculateHandler(IBusClient busClient, IBackgroundJobClient jobClient, ICalculator calculator, ILogger<CalculateHandler> logger)
        {
            _busClient = busClient;
            _jobClient = jobClient;
            _caculator = calculator;
            _logger = logger;
        }

        public void Handle(LdpTicketedMessage message)
        {
            Handle handle = _caculator.Calculate(message);
            if (handle == Internal.Handle.Winner)
            {
                _jobClient.Enqueue<IExecuterDispatcher<AwardingExecuter>>(executer => executer.DispatchAsync(new AwardingExecuter(message.LdpOrderId, message.LdpVenderId, message.LvpOrder)));
            }
            else if (handle == Internal.Handle.Losing)
            {
                _busClient.PublishAsync(new LdpAwardedMessage
                {
                    AftertaxAmount = 0,
                    BonusAmount = 0,
                    LdpOrderId = message.LdpOrderId,
                    LdpVenderId = message.LdpVenderId,
                    LvpOrder = message.LvpOrder,
                    Status = OrderStatus.TicketLosing
                }, context =>
                {
                    context.UsePublishConfiguration(configuration =>
                    {
                        configuration.OnDeclaredExchange(exchange =>
                        {
                            exchange.WithName("Baibaocp.LotteryVender")
                                    .WithType(ExchangeType.Topic)
                                    .WithAutoDelete(false);
                        });
                        configuration.WithRoutingKey(RoutingkeyConsts.Awards.Completed.Loseing);
                    });
                });
            }
            else
            {
                _jobClient.Schedule<CalculateHandler>(handler => handler.Handle(message), TimeSpan.FromMinutes(30));
            }
        }
    }
}
