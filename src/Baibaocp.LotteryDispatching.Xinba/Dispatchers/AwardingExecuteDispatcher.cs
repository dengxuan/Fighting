using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Xinba.Abstractions;
using Baibaocp.LotteryDispatching.Xinba.Extensions;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities;
using Fighting.Extensions;
using Fighting.Storaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Xinba.Dispatchers
{
    public class AwardingExecuteDispatcher : XinbaDispatcher<QueryingDispatchMessage>, IQueryingDispatcher
    {

        private readonly ILogger<AwardingExecuteDispatcher> _logger;

        private readonly IOrderingApplicationService _orderingApplicationService;

        public AwardingExecuteDispatcher(DispatcherConfiguration options, ILogger<AwardingExecuteDispatcher> logger, IOrderingApplicationService orderingApplicationService) : base(options, "1002", logger)
        {
            _logger = logger;
            _orderingApplicationService = orderingApplicationService;
        }
        public async Task<IQueryingHandle> DispatchAsync(QueryingDispatchMessage message)
        {
            try
            {
                XDocument content = new XDocument();
                XDocument rescontent = new XDocument();
                rescontent = await Send(message, "1002");
                bool handle = Verify(rescontent, out content);
                if (handle)
                {
                    XElement xml = content.Root;
                    XElement records = xml.Element("records");
                    foreach (XElement record in records.Elements("record"))
                    {
                        string id = record.Element("id").Value;
                        if (id == message.LdpOrderId)
                        {
                            var order = await _orderingApplicationService.FindOrderAsync(id);
                            int Bonus = int.Parse(record.Element("bonusValue").Value);
                            int Count = int.Parse(record.Element("bonusCount").Value);
                            int singleBonus = (Bonus / Count) / order.InvestTimes;
                            double tax = 0;
                            double AfterTacBonusAmount = 0;
                            if (singleBonus > 1000000)
                            {
                                tax = singleBonus * 0.2;
                                AfterTacBonusAmount = (singleBonus - tax) * Count * order.InvestTimes;
                            }
                            else
                            {
                                AfterTacBonusAmount = (double)Bonus;
                            }
                            return new WinningHandle(Bonus, (int)AfterTacBonusAmount);
                        }
                        else {
                            return new WaitingHandle();
                        }
                    }
                }
                return new WaitingHandle();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
                return new WaitingHandle();
            }
        }

        protected override XDocument BuildRequest(QueryingDispatchMessage message)
        {
            XDocument content = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            content.Add(new XElement("body", new XElement("messageId",message.LdpOrderId)));
            return content;
        }
    }
}
