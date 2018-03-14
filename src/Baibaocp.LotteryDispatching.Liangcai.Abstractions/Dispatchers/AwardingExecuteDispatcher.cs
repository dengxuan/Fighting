using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Liangcai.Liangcai;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Liangcai.Dispatchers
{
    public class AwardingExecuteDispatcher : LiangcaiExecuteDispatcher<QueryingExecuteMessage>, IAwardingExecuteDispatcher
    {

        private readonly ILogger<AwardingExecuteDispatcher> _logger;

        public AwardingExecuteDispatcher(DispatcherConfiguration options, ILogger<AwardingExecuteDispatcher> logger) : base(options, "111", logger)
        {
            _logger = logger;
        }

        protected override string BuildRequest(QueryingExecuteMessage executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.LdpOrderId)
            };

            return string.Join("_", values);
        }

        public override async Task<IExecuteHandle> DispatchAsync(QueryingExecuteMessage executer)
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
                return new WinningHandle(0, 0);
            }
            // TODO: Log here and notice to admin
            return new WaitingHandle();
        }
    }
}
