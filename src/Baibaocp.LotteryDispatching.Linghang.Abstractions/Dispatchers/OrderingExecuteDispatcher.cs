using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Linghang.Abstractions.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Suicai.Abstractions.Extensions;
using Baibaocp.LotteryOrdering.Linghang.Extensions;
using Baibaocp.Storaging.Entities.Extensions;
using Fighting.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Linghang.Abstractions.Dispatchers
{
    public class OrderingExecuteDispatcher : LinghangDispatcher<OrderingDispatchMessage>, IOrderingDispatcher
    {

        private readonly ILogger<OrderingExecuteDispatcher> _logger;

        public OrderingExecuteDispatcher(DispatcherConfiguration options, ILogger<OrderingExecuteDispatcher> logger) : base(options, logger, "101")
        {
            _logger = logger;
        }

        protected override string BuildRequest(OrderingDispatchMessage executer)
        {
            OrderSending ordersend = new OrderSending();
            ordersend.gameId = executer.LvpOrder.LotteryId.ToSuicaiLottery();
            ordersend.issue = executer.LvpOrder.IssueNumber.FromIssueNumber(executer.LvpOrder.LotteryId);
            ordersend.orderList = new List<order>();
            order order = new order()
            {
                orderId = executer.LdpOrderId,
                timeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString(),
                ticketMoney = (executer.LvpOrder.InvestAmount / 100).ToString(),
                betCount = "1",
                betDetail = executer.LvpOrder.InvestCode.ToSuicaicode(executer)
            };
            ordersend.orderList.Add(order);
            return JsonExtensions.ToJsonString(ordersend);
        }

        public Task<IOrderingHandle> DispatchAsync(OrderingDispatchMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
