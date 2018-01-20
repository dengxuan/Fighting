using Baibaocp.Storaging.Entities;
using Baibaocp.LotteryDispatcher.Core.Executers;
using Baibaocp.LotteryOrdering.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiAwardingExecuteHandler : ShanghaiExecuteHandler<AwardingExecuter>
    {
        private readonly IBusClient _client;

        private readonly ILogger<ShanghaiAwardingExecuteHandler> _logger;

        public ShanghaiAwardingExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory, IBusClient client) : base(options, loggerFactory, "111")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiAwardingExecuteHandler>();
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

        public override async Task<Handle> HandleAsync(AwardingExecuter executer)
        {
            string xml = await Send(executer);
            XDocument document = XDocument.Parse(xml);

            string Status = document.Element("ActionResult").Element("xCode").Value;
            string value = document.Element("ActionResult").Element("xValue").Value;
            if (Status.Equals("0"))
            {
                string[] values = value.Split('_');
                AwardedMessage awardedMessage = new AwardedMessage
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
                return Handle.Winning;
            }
            // TODO: Log here and notice to admin
            return Handle.Waiting;
        }
    }
}
