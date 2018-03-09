using Baibaocp.LotteryDispatching.MessageServices;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
using Baibaocp.LotteryDispatching.Suicai.Ticketing;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities;
using Fighting.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Awarding
{
    public class AwardingExecuteHandler : ExecuteHandler<AwardingMessage>
    {

        private readonly ILogger<AwardingExecuteHandler> _logger;

        public AwardingExecuteHandler(DispatcherOptions options, ILoggerFactory loggerFactory) : base(options, loggerFactory, "111")
        {
            _logger = loggerFactory.CreateLogger<AwardingExecuteHandler>();
        }

        protected override string BuildRequest(AwardingMessage executer)
        {
            OrderTicket Ticket = new OrderTicket();
            Ticket.orderList = new List<Ticket>();
            Ticket tc = new Ticket() { orderId = executer.LdpOrderId };
            return JsonExtensions.ToJsonString(Ticket);
        }

        public override async Task<IHandle> HandleAsync(AwardingMessage executer)
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
                        return HandleHelper.Waiting();
                    }
                    else if (Status.Equals("1"))
                    {
                        return HandleHelper.Loseing();
                    }
                    else if (Status.Equals("2"))
                    {
                        if (executer.LvpOrder.LotteryId == (int)LotteryTypes.GxSyxw)
                        {
                            LdpAwardedMessage awardedMessage = new LdpAwardedMessage
                            {
                                LvpOrder = executer.LvpOrder,
                                LdpOrderId = executer.LdpOrderId,
                                LdpVenderId = executer.LdpVenderId,
                                Status = OrderStatus.TicketWinning,
                                BonusAmount = (int)(Convert.ToDecimal(json["totalPrize"]) * 100)
                            };
                            return HandleHelper.Winning();
                        }
                    }
                    else if (Status.Equals("3"))
                    {
                        LdpAwardedMessage awardedMessage = new LdpAwardedMessage
                        {
                            LvpOrder = executer.LvpOrder,
                            LdpOrderId = executer.LdpOrderId,
                            LdpVenderId = executer.LdpVenderId,
                            Status = OrderStatus.TicketWinning,
                            BonusAmount = (int)(Convert.ToDecimal(json["totalPrize"]) * 100)
                        };
                        return HandleHelper.Winning();
                    }
                    else {
                        return HandleHelper.Waiting();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            // TODO: Log here and notice to admin
            return HandleHelper.Winning();
        }
    }
}
