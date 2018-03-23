using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
using Fighting.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Dispatchers
{
    public class AwardingExecuteDispatcher : SuicaiLotteryDispatcher<QueryingDispatchMessage>, IQueryingDispatcher
    {

        private readonly ILogger<AwardingExecuteDispatcher> _logger;

        public AwardingExecuteDispatcher(DispatcherConfiguration options, ILogger<AwardingExecuteDispatcher> logger) : base(options, logger, "111")
        {
            _logger = logger;
        }

        protected override string BuildRequest(QueryingDispatchMessage executer)
        {
            OrderTicket Ticket = new OrderTicket();
            Ticket.orderList = new List<Ticket>();
            Ticket tc = new Ticket() { orderId = executer.LdpOrderId.ToString() };
            return JsonExtensions.ToJsonString(Ticket);
        }

        public async Task<IQueryingHandle> DispatchAsync(QueryingDispatchMessage executer)
        {
            try
            {
                string jsoncontent = await Send(executer);
                JObject jarr = JObject.Parse(jsoncontent);
                if (jarr.HasValues)
                {
                    var json = jarr["orderList"][0];

                    string Status = json["status"].ToString();
                    if (Status.Equals("0"))
                    {
                        return new WaitingHandle();
                    }
                    else if (Status.Equals("1"))
                    {
                        return new LoseingHandle();
                    }
                    else if (Status.Equals("2"))
                    {
                        //if (executer.LvpOrder.LotteryId == (int)LotteryTypes.GxSyxw)
                        //{
                        //    //LdpAwardedMessage awardedMessage = new LdpAwardedMessage
                        //    //{
                        //    //    LvpOrder = executer.LvpOrder,
                        //    //    LdpOrderId = executer.LdpOrderId,
                        //    //    LdpVenderId = executer.LdpVenderId,
                        //    //    Status = OrderStatus.TicketWinning,
                        //    //    BonusAmount = (int)(Convert.ToDecimal(json["totalPrize"]) * 100)
                        //    //};
                        //    return new Winning((int)(Convert.ToDecimal(json["totalPrize"]) * 100), (int)(Convert.ToDecimal(json["totalPrize"]) * 100));
                        //}
                    }
                    else if (Status.Equals("3"))
                    {
                        //LdpAwardedMessage awardedMessage = new LdpAwardedMessage
                        //{
                        //    LvpOrder = executer.LvpOrder,
                        //    LdpOrderId = executer.LdpOrderId,
                        //    LdpVenderId = executer.LdpVenderId,
                        //    Status = OrderStatus.TicketWinning,
                        //    BonusAmount = (int)(Convert.ToDecimal(json["totalPrize"]) * 100)
                        //};
                        return new WinningHandle((int)(Convert.ToDecimal(json["totalPrize"]) * 100), (int)(Convert.ToDecimal(json["totalPrize"]) * 100));
                    }
                    else
                    {
                        return new WaitingHandle();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            // TODO: Log here and notice to admin
            return new WaitingHandle();
        }
    }
}
