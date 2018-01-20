using Baibaocp.Storaging.Entities.Extensions;
using Baibaocp.LotteryDispatcher.Core.Executers;
using Baibaocp.LotteryDispatcher.Extensions;
using Baibaocp.LotteryOrdering.Shanghai.Extensions;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatcher.Shanghai.Handlers
{
    public class ShanghaiOrderingExecuteHandler : ShanghaiExecuteHandler<OrderingExecuter>
    {
        private readonly IBusClient _publisher;

        private readonly ILogger<ShanghaiOrderingExecuteHandler> _logger;

        public ShanghaiOrderingExecuteHandler(ShanghaiDispatcherOptions options, ILoggerFactory loggerFactory, IBusClient publisher) : base(options, loggerFactory, "101")
        {
            _logger = loggerFactory.CreateLogger<ShanghaiOrderingExecuteHandler>();
            _publisher = publisher;
        }

        protected override string BuildRequest(OrderingExecuter executer)
        {
            /*
             
            var order = executer.LvpOrders.Select(selector => new { LotteryId = selector.LotteryId, LotteryPlayId = selector.LotteryPlayId, IssueNumber = selector.IssueNumber, InvestTimes = selector.InvestTimes, InvestType = selector.InvestType }).FirstOrDefault();
            var amount = executer.LvpOrders.Sum(selector => selector.InvestAmount);
            string codes = string.Join(";", executer.LvpOrders.Select(selector => ShanghaiJcCode.ReturnShanghaiCode(selector.InvestCode, selector.LotteryId, selector.LotteryPlayId)));

             */
            string[] values = new string[]
            {
                string.Format("OrderID={0}", executer.LdpOrderId),
                string.Format("LotID={0}", executer.LvpOrder.LotteryId.ToLottery()),
                string.Format("LotIssue={0}", executer.LvpOrder.IssueNumber ?? 0),
                string.Format("LotMoney={0}", executer.LvpOrder.InvestAmount / 100),
                string.Format("LotCode={0}", ShanghaiJcCode.ReturnShanghaiCode(executer.LvpOrder.InvestCode, executer.LvpOrder.LotteryId, executer.LvpOrder.LotteryPlayId)),
                string.Format("LotMulti={0}", executer.LvpOrder.InvestTimes),
                string.Format("Attach={0}", ""),
                string.Format("OneMoney={0}", executer.LvpOrder.InvestType ? "3":"2")
            };
            return string.Join("_", values);
        }

        public override async Task<Handle> HandleAsync(OrderingExecuter executer)
        {
            try
            {
                string xml = await Send(executer);
                XDocument document = XDocument.Parse(xml);

                string Status = document.Element("ActionResult").Element("xCode").Value;
                _logger.LogInformation("Response Status: {0}", Status);
                if (Status.IsIn("0", "1", "1008"))
                {
                    return Handle.Accepted;
                }
                else if (Status.IsIn("1003", "1011", "1014"))
                {
                    // TODO: Log here and notice to admin
                    return Handle.Waiting;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            return Handle.Rejected;
        }
    }
}
