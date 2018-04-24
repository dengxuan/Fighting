using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
using Baibaocp.Storaging.Entities;
using Fighting.Extensions;
using Fighting.Json;
using Fighting.Storaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Dispatchers
{
    public class QueryingExecuteDispatcher : SuicaiLotteryDispatcher<QueryingDispatchMessage>, IQueryingDispatcher
    {

        private readonly ILogger<QueryingExecuteDispatcher> _logger;

        public QueryingExecuteDispatcher(DispatcherConfiguration options, ILogger<QueryingExecuteDispatcher> logger) : base(options, logger, "200009")
        {
            _logger = logger;
        }

        protected override string BuildRequest(QueryingDispatchMessage executer)
        {
            OrderTicket Ticket = new OrderTicket();
            Ticket.orderList = new List<Ticket>();
            Ticket tc = new Ticket() { orderId = executer.LdpOrderId.ToString() };
            Ticket.orderList.Add(tc);
            return JsonExtensions.ToJsonString(Ticket);
        }


        public async Task<IQueryingHandle> DispatchAsync(QueryingDispatchMessage message)
        {
            try
            {
                string content = string.Empty;
                string rescontent = await Send(message);
                bool handle = Verify(rescontent, out content);
                _logger.LogInformation(content);
                if (handle)
                {
                    JObject jarr = JObject.Parse(content);
                    if (jarr.HasValues)
                    {
                        var json = jarr["resultList"].First;

                        string Status = json["status"].ToString();
                        if (Status.IsIn("0", "1"))
                        {
                            return new WaitingHandle();
                        }
                        else if (Status.Equals("2"))
                        {
                            if (message.QueryingType == QueryingTypes.Awarding)
                            {
                                string awardStatus = json["awardStatus"].ToString();
                                if (awardStatus.Equals("0"))
                                {
#if DEBUG
                                    if (message.QueryingType == QueryingTypes.Awarding)
                                    {
                                        return new WinningHandle(10000, 10000);
                                    }
                                    else
                                    {
                                        return new WaitingHandle();
                                    }
#else
                return new WaitingHandle();
#endif
                                }
                                else if (awardStatus.Equals("1"))
                                {
                                    return new LoseingHandle();
                                }
                                else if (awardStatus.Equals("2") || awardStatus.Equals("3"))
                                {
                                    int bonusAmount = (int)(Convert.ToDecimal(json["totalPrize"].ToString()) * 100);
                                    int totalTax = (int)(Convert.ToDecimal(json["totalTax"].ToString()) * 100);
                                    int aftertaxBonusAmount = bonusAmount - totalTax;
                                    return new WinningHandle(bonusAmount, aftertaxBonusAmount);
                                }
                                else
                                {
#if DEBUG
                                    if (message.QueryingType == QueryingTypes.Awarding)
                                    {
                                        return new WinningHandle(10000, 10000);
                                    }
                                    else
                                    {
                                        return new WaitingHandle();
                                    }
#else
                return new WaitingHandle();
#endif
                                }
                            }
                            else
                            {
                                string ticketId = json["tickSn"].ToString();
                                DateTime tickettime = DateTime.Now;
                                return new SuccessHandle(ticketId, tickettime, "");
                            }
                        }
                        else
                        {
                            return new FailureHandle();
                        }
                    }
                }
                else
                {
                    return new FailureHandle();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            return new WaitingHandle();
        }
    }
}
