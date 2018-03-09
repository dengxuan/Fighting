using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Liangcai.Handlers
{
    public class AwardingExecuteHandler : ExecuteHandler<QueryingExecuteMessage>
    {

        private readonly ILogger<AwardingExecuteHandler> _logger;

        public AwardingExecuteHandler(DispatcherOptions options, ILoggerFactory loggerFactory) : base(options, loggerFactory, "111")
        {
            _logger = loggerFactory.CreateLogger<AwardingExecuteHandler>();
        }

        protected override string BuildRequest(QueryingExecuteMessage executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.LdpOrderId)
            };

            return string.Join("_", values);
        }

        public override async Task<IHandle> HandleAsync(QueryingExecuteMessage executer)
        {
            string xml = await Send(executer);
            XDocument document = XDocument.Parse(xml);

            string Status = document.Element("ActionResult").Element("xCode").Value;
            string value = document.Element("ActionResult").Element("xValue").Value;
            if (Status.Equals("0"))
            {
                string[] values = value.Split('_');
                //LdpAwardedMessage awardedMessage = new LdpAwardedMessage
                //{
                //    LvpOrder = executer.LvpOrder,
                //    LdpOrderId = executer.LdpOrderId,
                //    LdpVenderId = executer.LdpVenderId,
                //    Status = OrderStatus.TicketWinning,
                //    BonusAmount = (int)(Convert.ToDecimal(values[2]) * 100)
                //};
                return new Winning();
            }
            // TODO: Log here and notice to admin
            return new Waiting();
        }
    }
}
