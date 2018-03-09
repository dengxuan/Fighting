using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Extensions;
using Baibaocp.LotteryDispatching.MessageServices;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
using Baibaocp.LotteryDispatching.Suicai.Abstractions.Extensions;
using Fighting.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Ordering
{
    public class OrderingExecuteHandler : ExecuteHandler<OrderingMessage>, IExecuteHandler<OrderingMessage>
    {
        private readonly IBusClient _publisher;

        private readonly ILogger<OrderingExecuteHandler> _logger;

        


        public OrderingExecuteHandler(DispatcherOptions options, ILoggerFactory loggerFactory, IBusClient publisher) : base(options, loggerFactory, "200008)")
        {
            _logger = loggerFactory.CreateLogger<OrderingExecuteHandler>();
            _publisher = publisher;
        }

        protected override string BuildRequest(OrderingMessage executer)
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

        public override async Task<IHandle> HandleAsync(OrderingMessage executer)
        {
            try
            {
                string jsoncontent = await Send(executer);
                JObject jarr = JObject.Parse(jsoncontent);
                if (jarr.HasValues) {
                    var json = jarr["orderList"][0];
                    string Status = json["status"].ToString();
                    _logger.LogInformation("Response Status: {0}", Status);
                    if (Status.Equals("0"))
                    {
                        return HandleHelper.Accept();
                    }
                    else if (Status.IsIn("-1"))
                    {
                        // TODO: Log here and notice to admin
                        return HandleHelper.Reject();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            return HandleHelper.Reject();
        }
    }
}
