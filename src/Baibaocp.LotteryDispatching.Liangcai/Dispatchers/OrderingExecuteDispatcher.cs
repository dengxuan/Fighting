using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Extensions;
using Baibaocp.LotteryDispatching.Liangcai.Liangcai;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Liangcai.Extensions;
using Baibaocp.Storaging.Entities.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Liangcai.Dispatchers
{
    public class OrderingExecuteDispatcher : LiangcaiDispatcher<OrderingDispatchMessage>, IOrderingDispatcher
    {
        private readonly ILogger<OrderingExecuteDispatcher> _logger;

        public OrderingExecuteDispatcher(DispatcherConfiguration options, ILogger<OrderingExecuteDispatcher> logger) : base(options, "101", logger)
        {
            _logger = logger;
        }

        protected override string BuildRequest(OrderingDispatchMessage message)
        {
            /*
             
            var order = executer.LvpOrders.Select(selector => new { LotteryId = selector.LotteryId, LotteryPlayId = selector.LotteryPlayId, IssueNumber = selector.IssueNumber, InvestTimes = selector.InvestTimes, InvestType = selector.InvestType }).FirstOrDefault();
            var amount = executer.LvpOrders.Sum(selector => selector.InvestAmount);
            string codes = string.Join(";", executer.LvpOrders.Select(selector => ShanghaiJcCode.ReturnShanghaiCode(selector.InvestCode, selector.LotteryId, selector.LotteryPlayId)));

             */
            string[] values = new string[]
            {
                string.Format("OrderID={0}", message.LdpOrderId),
                string.Format("LotID={0}", message.LvpOrder.LotteryId.ToLottery()),
                string.Format("LotIssue={0}", message.LvpOrder.IssueNumber ?? 0),
                string.Format("LotMoney={0}", message.LvpOrder.InvestAmount / 100),
                string.Format("LotCode={0}", ShanghaiJcCode.ReturnShanghaiCode(message.LvpOrder.InvestCode, message.LvpOrder.LotteryId, message.LvpOrder.LotteryPlayId)),
                string.Format("LotMulti={0}", message.LvpOrder.InvestTimes),
                string.Format("Attach={0}", ""),
                string.Format("OneMoney={0}", message.LvpOrder.InvestType ? "3":"2")
            };
            return string.Join("_", values);
        }

        public async Task<IOrderingHandle> DispatchAsync(OrderingDispatchMessage message)
        {
            try
            {
                string xml = await Send(message, "101");
                XDocument document = XDocument.Parse(xml);

                string Status = document.Element("ActionResult").Element("xCode").Value;
                _logger.LogInformation("Response Status: {0}", Status);
                if (Status.IsIn("0", "1", "1008"))
                {
                    return new AcceptedHandle();
                }
                else if (Status.IsIn("1003", "1011", "1014"))
                {
                    // TODO: Log here and notice to admin
                    return new RejectedHandle(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            return new RejectedHandle();
        }
    }
}
