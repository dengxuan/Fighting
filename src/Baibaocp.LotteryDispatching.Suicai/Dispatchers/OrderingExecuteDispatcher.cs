using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
using Baibaocp.LotteryDispatching.Suicai.Abstractions.Extensions;
using Fighting.Extensions;
using Fighting.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Dispatchers
{
    public class OrderingExecuteDispatcher : SuicaiLotteryDispatcher<OrderingDispatchMessage>, IOrderingDispatcher
    {
        private readonly IBusClient _publisher;

        private readonly ILogger<OrderingExecuteDispatcher> _logger;

        public OrderingExecuteDispatcher(DispatcherConfiguration options, ILogger<OrderingExecuteDispatcher> logger, IBusClient publisher) : base(options, logger, "200008")
        {
            _logger = logger;
            _publisher = publisher;
        }

        protected override string BuildRequest(OrderingDispatchMessage executer)
        {
            OrderSending ordersend = new OrderSending();
            ordersend.gameId = executer.LvpOrder.LotteryId.ToSuicaiLottery();
            ordersend.issue = executer.LvpOrder.IssueNumber.FromIssueNumber(executer.LvpOrder.LotteryId);
            ordersend.orderList = new List<order>();
            order order = new order()
            {
                orderId = executer.LdpOrderId.ToString(),
                timeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString(),
                ticketMoney = (executer.LvpOrder.InvestAmount / 100).ToString(),
                betCount = "1",
                betDetail = executer.LvpOrder.InvestCode.ToSuicaicode(executer)
            };
            ordersend.orderList.Add(order);
            return JsonExtensions.ToJsonString(ordersend);
        }

        public async Task<IOrderingHandle> DispatchAsync(OrderingDispatchMessage executer)
        {
            try
            {
                string content = string.Empty;
                string rescontent = await Send(executer);
                bool handle = Verify(rescontent, out content);
                if (handle)
                {
                    JObject jarr = JObject.Parse(content);
                    if (jarr.HasValues)
                    {
                        var json = jarr["orderList"][0];
                        string Status = json["status"].ToString();
                        _logger.LogInformation("Response Status: {0}", Status);
                        if (Status.Equals("0"))
                        {
                            return new AcceptedHandle();
                        }
                        else if (Status.IsIn("-1"))
                        {
                            // TODO: Log here and notice to admin
                            return new RejectedHandle();
                        }
                    }
                }
                else {
                    return new RejectedHandle();
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
