using Baibaocp.LotteryDispatching.Executers;
using Baibaocp.LotteryDispatching.MessageServices;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Liangcai.Handlers
{
    public class AwardingExecuteHandler : ExecuteHandler<AwardingExecuter>
    {
        private readonly IBusClient _client;

        private readonly ILogger<AwardingExecuteHandler> _logger;

        public AwardingExecuteHandler(DispatcherOptions options, ILoggerFactory loggerFactory, IBusClient client) : base(options, loggerFactory, "111")
        {
            _logger = loggerFactory.CreateLogger<AwardingExecuteHandler>();
            _client = client;
        }

        protected override string BuildRequest(AwardingExecuter executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.LdpOrderId)
            };

            return string.Join("_", values);
        }

        public override async Task<MessageHandle> HandleAsync(AwardingExecuter executer)
        {
            string xml = await Send(executer);
            XDocument document = XDocument.Parse(xml);

            string Status = document.Element("ActionResult").Element("xCode").Value;
            string value = document.Element("ActionResult").Element("xValue").Value;
            if (Status.Equals("0"))
            {
                string[] values = value.Split('_');
                LdpAwardedMessage awardedMessage = new LdpAwardedMessage
                {
                    LvpOrder = executer.LvpOrder,
                    LdpOrderId = executer.LdpOrderId,
                    LdpVenderId = executer.LdpVenderId,
                    Status = OrderStatus.TicketWinning,
                    BonusAmount = (int)(Convert.ToDecimal(values[2]) * 100)
                };
                await _client.PublishAsync(awardedMessage,context=> 
                {
                    context.UsePublishConfiguration(configuration => 
                    {
                        configuration.OnDeclaredExchange(exchange => 
                        {
                            exchange.WithName("Baibaocp.LotteryVender")
                                    .WithAutoDelete(false)
                                    .WithDurability(true)
                                    .WithType(ExchangeType.Topic);
                        });
                        configuration.WithRoutingKey(RoutingkeyConsts.Awards.Completed.Winning);
                    });
                });
                return MessageHandle.Winning;
            }
            // TODO: Log here and notice to admin
            return MessageHandle.Waiting;
        }
    }
}
