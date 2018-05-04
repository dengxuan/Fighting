using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Xinba.Abstractions;
using Baibaocp.LotteryDispatching.Xinba.Extensions;
using Fighting.Extensions;
using Fighting.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Xinba.Dispatchers
{
    public class OrderingExecuteDispatcher : XinbaDispatcher<OrderingDispatchMessage>, IOrderingDispatcher
    {
        private readonly IBusClient _publisher;

        private readonly ILogger<OrderingExecuteDispatcher> _logger;

        public OrderingExecuteDispatcher(DispatcherConfiguration options, ILogger<OrderingExecuteDispatcher> logger, IBusClient publisher) : base(options, "1000", logger)
        {
            _logger = logger;
            _publisher = publisher;
        }



        public async Task<IOrderingHandle> DispatchAsync(OrderingDispatchMessage message)
        {
            try
            {
                XDocument content = new XDocument();
                XDocument rescontent = await Send(message,"1000");
                bool handle = Verify(rescontent, out content);
                if (handle)
                {
                    XElement xml = content.Root;
                    XElement records = xml.Element("records");
                    foreach (XElement record in records.Elements("record"))
                    {
                        if (record.Element("result").Value == "0")
                        {
                            return new AcceptedHandle();
                        }
                        else if (record.Element("result").Value.IsIn("200001", "200006"))
                        {
                            return new RejectedHandle(true);
                        }
                        else
                        {
                            return new RejectedHandle();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
            }
            return new RejectedHandle();
        }

        protected override XDocument BuildRequest(OrderingDispatchMessage message)
        {
            XDocument content = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement lot = new XElement("lotteryId", message.LvpOrder.LotteryId.ToXinbaLottery());
            XElement records = new XElement("records");
            records.Add(new XElement("record", new XElement[]
            {
                new XElement("id",message.LdpOrderId.ToString()),
                new XElement("lotterySaleId",Convert.ToInt32(message.LvpOrder.LotteryPlayId.ToXinbaPlay())),
                new XElement("phone","18210416976"),
                new XElement("idCard","130631198905160037"),
                new XElement("code",message.LvpOrder.InvestCode.ToCastcode(message.LvpOrder.LotteryPlayId,message.LvpOrder.LotteryId)),
                new XElement("money",message.LvpOrder.InvestAmount),
                new XElement("timesCount",message.LvpOrder.InvestTimes),
                new XElement("issueCount",1),
                new XElement("investCount",message.LvpOrder.InvestCount),
                new XElement("investType",Convert.ToInt32(message.LvpOrder.InvestType))
            }));
            XElement issue = new XElement("issue", message.LvpOrder.IssueNumber.FromIssueNumber(message.LvpOrder.LotteryId));
            content.Add(new XElement("body", lot, issue, records));
            return content;
        }
    }
}
