using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
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

        private readonly StorageOptions _storageOptions;

        private readonly ILogger<QueryingExecuteDispatcher> _logger;

        public QueryingExecuteDispatcher(DispatcherConfiguration options, StorageOptions storageOptions, ILogger<QueryingExecuteDispatcher> logger) : base(options, logger, "200009")
        {
            _logger = logger;
            _storageOptions = storageOptions;
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
                string content = string.Empty;
                string rescontent = await Send(executer);
                bool handle = Verify(rescontent, out content);
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
                            string ticketId = json["tickSn"].ToString();
                            DateTime tickettime = DateTime.Now;
                            return new SuccessHandle(ticketId, tickettime, "");
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
