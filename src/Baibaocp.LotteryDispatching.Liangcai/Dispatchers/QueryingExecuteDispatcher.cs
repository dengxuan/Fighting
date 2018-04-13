using Baibaocp.LotteryDispatcher.Liangcai;
using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Liangcai.Liangcai;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.Storaging.Entities.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Baibaocp.LotteryDispatching.Liangcai.Handlers
{
    public class QueryingExecuteDispatcher : LiangcaiDispatcher<QueryingDispatchMessage>, IQueryingDispatcher
    {
        private readonly Dictionary<QueryingTypes, string> _commands = new Dictionary<QueryingTypes, string>
        {
            { QueryingTypes.Ticketing, "102" },
            { QueryingTypes.Awarding, "111" }
        };
        private readonly ILogger<QueryingExecuteDispatcher> _logger;

        public QueryingExecuteDispatcher(DispatcherConfiguration options, ILogger<QueryingExecuteDispatcher> logger) : base(options, "102", logger)
        {
            _logger = logger;
        }

        /// <summary>  
        /// zlib.net 解压函数
        /// </summary>  
        /// <param name="strSource">带解压数据源</param>  
        /// <returns>解压后的数据</returns>  
        public static string DeflateDecompress(string strSource)
        {
            byte[] Buffer = Convert.FromBase64String(strSource); // 解base64  
            using (MemoryStream intms = new MemoryStream(Buffer))
            {
                using (zlib.ZInputStream inZStream = new zlib.ZInputStream(intms))
                {
                    int size = short.MaxValue;
                    byte[] buffer = new byte[size];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        while ((size = inZStream.read(buffer, 0, size)) != -1)
                        {
                            ms.Write(buffer, 0, size);
                        }
                        inZStream.Close();
                        return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                    }
                }
            }
        }
        protected override string BuildRequest(QueryingDispatchMessage executer)
        {
            string[] values = new string[]
            {
                    string.Format("OrderID={0}", executer.LdpOrderId)
            };
            return string.Join("_", values);
        }

        protected IList<(string Id, DateTime? Time, string Odds)> ResolveTicketResults(int lotteryId, string xml)
        {
            XElement element = XElement.Parse(xml);
            IEnumerable<XElement> bills = element.Elements("bill");
            List<(string Id, DateTime? Time, string Odds)> results = new List<(string Id, DateTime? Time, string Odds)>();
            foreach (var bill in bills)
            {
                StringBuilder sb = new StringBuilder();
                IEnumerable<XElement> matches = bill.Elements("match");
                foreach (var match in matches)
                {
                    string attr = $"20{match.Attribute("id").Value}";
                    DateTime date = DateTime.ParseExact(attr.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture);
                    string @event = attr.Substring(8);
                    string id = $"{date.ToString("yyyyMMdd")}{(date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)date.DayOfWeek)}{@event}";
                    int rateCount = 0;
                    int playId = lotteryId;

                    if (lotteryId == 20205)
                    {
                        playId = match.Attribute("playid").Value.ToBaibaoLottery();
                        id = id + "-" + playId.ToString();
                    }
                    if (playId == 20206)
                    {
                        rateCount = int.Parse(match.Attribute("rq").Value);
                    }
                    string odds = match.Value.Replace('=', '*').Replace('|', '#');
                    sb.Append($"{id}@{rateCount}|{odds}#^");
                }
                string ticketedNumber = bill.Attribute("id").Value;
                DateTime? ticketedTime = null;
                string ticketedOdds = sb.ToString();
                if (DateTime.TryParse(bill.Attribute("billtime").Value, out DateTime time))
                {
                    ticketedTime = time;
                }
                results.Add((ticketedNumber, ticketedTime, sb.ToString()));
            }
            return results;
        }

        public async Task<IQueryingHandle> DispatchAsync(QueryingDispatchMessage message)
        {
            if (_commands.TryGetValue(message.QueryingType, out string command))
            {
                string xml = await Send(message, command);
                XDocument document = XDocument.Parse(xml);

                string Status = document.Element("ActionResult").Element("xCode").Value;
                string value = document.Element("ActionResult").Element("xValue").Value;

                if (Status.Equals("0") && message.QueryingType == QueryingTypes.Awarding)
                {
                    string[] values = value.Split('_');
                    return new WinningHandle((int)(Convert.ToDecimal(values[1]) * 100), (int)(Convert.ToDecimal(values[2]) * 100));
                }
                if (Status.Equals("1") && message.QueryingType == QueryingTypes.Ticketing)
                {
                    string odds = document.Element("ActionResult").Element("xValue").Value.Split('_')[3];
                    string oddsXml = DeflateDecompress(odds);
                    if (message.LotteryId > 20200)
                    {
                        IList<(string Id, DateTime? Time, string Odds)> results = ResolveTicketResults(message.LotteryId, oddsXml);
                        return new SuccessHandle(results[0].Id, results[0].Time, results[0].Odds);
                    }
                    return new SuccessHandle(oddsXml, DateTime.Now);
                }
                else if (Status.Equals("2003"))
                {
                    return new FailureHandle();
                }
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
            throw new ArgumentException("No command found");
        }
    }
}
