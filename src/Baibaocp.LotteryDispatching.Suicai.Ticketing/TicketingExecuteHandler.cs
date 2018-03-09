using Baibaocp.LotteryDispatching.Extensions;
using Baibaocp.LotteryDispatching.MessageServices;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Baibaocp.LotteryDispatching.Suicai.Abstractions;
using Fighting.Json;
using Fighting.Storaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Ticketing
{
    public class TicketingExecuteHandler : ExecuteHandler<TicketingMessage>
    {

        private readonly StorageOptions _storageOptions;

        private readonly ILogger<TicketingExecuteHandler> _logger;

        public TicketingExecuteHandler(DispatcherOptions options, StorageOptions storageOptions, ILoggerFactory loggerFactory) : base(options, loggerFactory, "102")
        {
            _logger = loggerFactory.CreateLogger<TicketingExecuteHandler>();
            _storageOptions = storageOptions;
        }

        protected override string BuildRequest(TicketingMessage executer)
        {
            OrderTicket Ticket = new OrderTicket();
            Ticket.orderList = new List<Ticket>();
            Ticket tc = new Ticket() { orderId = executer.LdpOrderId };
            return JsonExtensions.ToJsonString(Ticket);
        }


        public override async Task<IHandle> HandleAsync(TicketingMessage executer)
        {
            try
            {
                string jsoncontent = await Send(executer);
                JObject jarr = JObject.Parse(jsoncontent);
                if (jarr.HasValues)
                {
                    var json = jarr["orderList"][0];

                    string Status = json["status"].ToString();
                    if (Status.IsIn("0", "1"))
                    {
                        return new Waiting();
                    }
                    else if (Status.Equals("2"))
                    {
                        return new Success();
                    }
                    else
                    {
                        return new Failure();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            return new Waiting();
        }
    }
}
