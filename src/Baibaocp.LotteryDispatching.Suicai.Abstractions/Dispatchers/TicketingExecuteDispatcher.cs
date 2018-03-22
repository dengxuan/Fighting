using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Extensions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
using Fighting.Json;
using Fighting.Storaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Dispatchers
{
    public class TicketingExecuteDispatcher : SuicaiLotteryDispatcher<QueryingExecuteMessage>, ITicketingDispatcher
    {

        private readonly StorageOptions _storageOptions;

        private readonly ILogger<TicketingExecuteDispatcher> _logger;

        public TicketingExecuteDispatcher(DispatcherConfiguration options, StorageOptions storageOptions, ILogger<TicketingExecuteDispatcher> logger) : base(options, logger, "200009")
        {
            _logger = logger;
            _storageOptions = storageOptions;
        }

        protected override string BuildRequest(QueryingExecuteMessage executer)
        {
            OrderTicket Ticket = new OrderTicket();
            Ticket.orderList = new List<Ticket>();
            Ticket tc = new Ticket() { orderId = executer.LdpOrderId };
            return JsonExtensions.ToJsonString(Ticket);
        }


        public async Task<IQueryingHandle> DispatchAsync(QueryingExecuteMessage executer)
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
                        if (Status.IsIn("0", "1"))
                        {
                            return new WaitingHandle();
                        }
                        else if (Status.Equals("2"))
                        {
                            return new SuccessHandle("1", "2");
                        }
                        else
                        {
                            return new FailureHandle();
                        }
                    }
                }
                else {
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
