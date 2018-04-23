using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryDispatching.Xinba.Abstractions;
using Baibaocp.LotteryDispatching.Xinba.Extensions;
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
    public class TicketingExecuteDispatcher : XinbaDispatcher<QueryingDispatchMessage>, IQueryingDispatcher
    {

        private readonly ILogger<TicketingExecuteDispatcher> _logger;

        public TicketingExecuteDispatcher(DispatcherConfiguration options, ILogger<TicketingExecuteDispatcher> logger) : base(options, "1003", logger)
        {
            _logger = logger;
        }
        public async Task<IQueryingHandle> DispatchAsync(QueryingDispatchMessage message)
        {
            try {
                XDocument content = new XDocument();
                XDocument rescontent = new XDocument();
                if (message.LotteryId > 20200) {
                    rescontent = await Send(message, "1020");
                }
                else {
                    rescontent = await Send(message, "1003");
                }
                bool handle = Verify(rescontent, out content);
                if (handle)
                {
                    XElement xml = content.Root;
                    XElement records = xml.Element("records");
                    foreach (XElement record in records.Elements("record"))
                    {
                        string result = record.Element("result").Value;
                        if (result == "0")
                        {
                            string ticketid = record.Element("ticketId").Value;
                            if (message.LotteryId > 20200)
                            {
                                string ticketodds = Odds(message.LotteryId, record.Element("info"));
                                return new SuccessHandle(ticketid, DateTime.Now, ticketodds);
                            }
                            return new SuccessHandle(ticketid, DateTime.Now);
                        }
                        else if (result == "200021")
                        {
                            return new WaitingHandle();
                        }
                        else if (result == "200020")
                        {
                            return new FailureHandle();
                        }
                        else {
                            return new WaitingHandle();
                        }
                    }
                }
                
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Request Exception:{0}", ex.Message);
                return new WaitingHandle();    
            }
            return new WaitingHandle();
            //throw new ArgumentException("No command found");
        }

        protected override XDocument BuildRequest(QueryingDispatchMessage message)
        {
            XDocument content = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement records = new XElement("records");
            records.Add(new XElement("record", new XElement("id", message.LdpOrderId)));
            content.Add(new XElement("body", records));
            return content;
        }


        protected static string Odds(int lotteryid, XElement info)
        {
            string newcode = string.Empty;
            foreach (XElement item in info.Elements("item"))
            {
                string[] eventidarr = new string[4];
                eventidarr = item.Element("id").Value.Split('_');
                string neweventid = string.Empty;
                if (lotteryid == (int)LotteryTypes.LcHun || lotteryid == (int)LotteryTypes.JcHun)
                {
                    neweventid = eventidarr[0] + eventidarr[1] + eventidarr[2] + "-" + Convert.ToInt32(eventidarr[3].ToBaibaoLottery()).ToString();
                }
                else
                {
                    neweventid = eventidarr[0] + eventidarr[1] + eventidarr[2];
                }
                string lotid = string.Empty;
                string newodds = string.Empty;

                if (lotteryid == (int)LotteryTypes.JcHun || lotteryid == (int)LotteryTypes.LcHun)
                {
                    lotid = Convert.ToInt32(eventidarr[3].ToBaibaoLottery()).ToString();
                }
                else
                {
                    lotid = lotteryid.ToString();
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcSpf).ToString())
                {
                    XElement v3 = item.Element("vs").Element("v3");
                    XElement v1 = item.Element("vs").Element("v1");
                    XElement v0 = item.Element("vs").Element("v0");
                    newodds = neweventid + "@0" + "|";
                    if (v3 != null)
                    {
                        newodds = newodds + "3*" + v3.Value + "#";
                    }
                    if (v1 != null)
                    {
                        newodds = newodds + "1*" + v1.Value + "#";
                    }
                    if (v0 != null)
                    {
                        newodds = newodds + "0*" + v0.Value + "#";
                    }
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcRqspf).ToString())
                {

                    XElement let = item.Element("letVs").Element("letPoint");
                    XElement letv3 = item.Element("letVs").Element("v3");
                    XElement letv1 = item.Element("letVs").Element("v1");
                    XElement letv0 = item.Element("letVs").Element("v0");
                    newodds = neweventid + "@" + let.Value + "|";
                    if (letv3 != null)
                    {
                        newodds = newodds + "3*" + letv3.Value + "#";
                    }
                    if (letv1 != null)
                    {
                        newodds = newodds + "1*" + letv1.Value + "#";
                    }
                    if (letv0 != null)
                    {
                        newodds = newodds + "0*" + letv0.Value + "#";
                    }
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcBf).ToString())
                {
                    XElement v10 = item.Element("score").Element("v10");
                    XElement v20 = item.Element("score").Element("v20");
                    XElement v21 = item.Element("score").Element("v21");
                    XElement v30 = item.Element("score").Element("v30");
                    XElement v31 = item.Element("score").Element("v31");
                    XElement v32 = item.Element("score").Element("v32");
                    XElement v40 = item.Element("score").Element("v40");
                    XElement v41 = item.Element("score").Element("v41");
                    XElement v42 = item.Element("score").Element("v42");
                    XElement v50 = item.Element("score").Element("v50");
                    XElement v51 = item.Element("score").Element("v51");
                    XElement v52 = item.Element("score").Element("v52");
                    XElement v01 = item.Element("score").Element("v01");
                    XElement v02 = item.Element("score").Element("v02");
                    XElement v12 = item.Element("score").Element("v12");
                    XElement v03 = item.Element("score").Element("v03");
                    XElement v13 = item.Element("score").Element("v13");
                    XElement v23 = item.Element("score").Element("v23");
                    XElement v04 = item.Element("score").Element("v04");
                    XElement v14 = item.Element("score").Element("v14");
                    XElement v24 = item.Element("score").Element("v24");
                    XElement v05 = item.Element("score").Element("v05");
                    XElement v15 = item.Element("score").Element("v15");
                    XElement v25 = item.Element("score").Element("v25");
                    XElement v00 = item.Element("score").Element("v00");
                    XElement v11 = item.Element("score").Element("v11");
                    XElement v22 = item.Element("score").Element("v22");
                    XElement v33 = item.Element("score").Element("v33");
                    XElement v90 = item.Element("score").Element("v90");
                    XElement v99 = item.Element("score").Element("v99");
                    XElement v09 = item.Element("score").Element("v09");
                    newodds = neweventid + "@0" + "|";
                    if (v10 != null)
                    {
                        newodds = newodds + "10*" + v10.Value + "#";
                    }
                    if (v20 != null)
                    {
                        newodds = newodds + "20*" + v20.Value + "#";
                    }
                    if (v21 != null)
                    {
                        newodds = newodds + "21*" + v21.Value + "#";
                    }
                    if (v30 != null)
                    {
                        newodds = newodds + "30*" + v30.Value + "#";
                    }
                    if (v31 != null)
                    {
                        newodds = newodds + "31*" + v31.Value + "#";
                    }
                    if (v32 != null)
                    {
                        newodds = newodds + "32*" + v32.Value + "#";
                    }
                    if (v40 != null)
                    {
                        newodds = newodds + "40*" + v40.Value + "#";
                    }
                    if (v41 != null)
                    {
                        newodds = newodds + "41*" + v41.Value + "#";
                    }
                    if (v42 != null)
                    {
                        newodds = newodds + "42*" + v42.Value + "#";
                    }
                    if (v50 != null)
                    {
                        newodds = newodds + "50*" + v50.Value + "#";
                    }
                    if (v51 != null)
                    {
                        newodds = newodds + "51*" + v51.Value + "#";
                    }
                    if (v52 != null)
                    {
                        newodds = newodds + "52*" + v52.Value + "#";
                    }
                    if (v01 != null)
                    {
                        newodds = newodds + "01*" + v01.Value + "#";
                    }
                    if (v02 != null)
                    {
                        newodds = newodds + "02*" + v02.Value + "#";
                    }
                    if (v12 != null)
                    {
                        newodds = newodds + "12*" + v12.Value + "#";
                    }
                    if (v03 != null)
                    {
                        newodds = newodds + "03*" + v03.Value + "#";
                    }
                    if (v13 != null)
                    {
                        newodds = newodds + "13*" + v13.Value + "#";
                    }
                    if (v23 != null)
                    {
                        newodds = newodds + "23*" + v23.Value + "#";
                    }
                    if (v04 != null)
                    {
                        newodds = newodds + "04*" + v04.Value + "#";
                    }
                    if (v14 != null)
                    {
                        newodds = newodds + "14*" + v14.Value + "#";
                    }
                    if (v24 != null)
                    {
                        newodds = newodds + "24*" + v24.Value + "#";
                    }
                    if (v05 != null)
                    {
                        newodds = newodds + "05*" + v05.Value + "#";
                    }
                    if (v15 != null)
                    {
                        newodds = newodds + "15*" + v15.Value + "#";
                    }
                    if (v25 != null)
                    {
                        newodds = newodds + "25*" + v25.Value + "#";
                    }
                    if (v00 != null)
                    {
                        newodds = newodds + "00*" + v00.Value + "#";
                    }
                    if (v11 != null)
                    {
                        newodds = newodds + "11*" + v11.Value + "#";
                    }
                    if (v22 != null)
                    {
                        newodds = newodds + "22*" + v22.Value + "#";
                    }
                    if (v33 != null)
                    {
                        newodds = newodds + "33*" + v33.Value + "#";
                    }
                    if (v90 != null)
                    {
                        newodds = newodds + "90*" + v90.Value + "#";
                    }
                    if (v99 != null)
                    {
                        newodds = newodds + "99*" + v99.Value + "#";
                    }
                    if (v09 != null)
                    {
                        newodds = newodds + "09*" + v09.Value + "#";
                    }
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcBqc).ToString())
                {
                    XElement v33 = item.Element("half").Element("v33");
                    XElement v30 = item.Element("half").Element("v30");
                    XElement v31 = item.Element("half").Element("v31");
                    XElement v11 = item.Element("half").Element("v11");
                    XElement v10 = item.Element("half").Element("v10");
                    XElement v13 = item.Element("half").Element("v13");
                    XElement v03 = item.Element("half").Element("v03");
                    XElement v01 = item.Element("half").Element("v01");
                    XElement v00 = item.Element("half").Element("v00");
                    newodds = neweventid + "@0" + "|";
                    if (v33 != null)
                    {
                        newodds = newodds + "33*" + v33.Value + "#";
                    }
                    if (v31 != null)
                    {
                        newodds = newodds + "31*" + v31.Value + "#";
                    }
                    if (v30 != null)
                    {
                        newodds = newodds + "30*" + v30.Value + "#";
                    }
                    if (v11 != null)
                    {
                        newodds = newodds + "11*" + v11.Value + "#";
                    }
                    if (v10 != null)
                    {
                        newodds = newodds + "10*" + v10.Value + "#";
                    }
                    if (v13 != null)
                    {
                        newodds = newodds + "13*" + v13.Value + "#";
                    }
                    if (v00 != null)
                    {
                        newodds = newodds + "00*" + v00.Value + "#";
                    }
                    if (v01 != null)
                    {
                        newodds = newodds + "01*" + v01.Value + "#";
                    }
                    if (v03 != null)
                    {
                        newodds = newodds + "03*" + v03.Value + "#";
                    }
                }
                if (lotid == Convert.ToInt32(LotteryTypes.JcZjq).ToString())
                {
                    XElement v0 = item.Element("goal").Element("v0");
                    XElement v1 = item.Element("goal").Element("v1");
                    XElement v2 = item.Element("goal").Element("v2");
                    XElement v3 = item.Element("goal").Element("v3");
                    XElement v4 = item.Element("goal").Element("v4");
                    XElement v5 = item.Element("goal").Element("v5");
                    XElement v6 = item.Element("goal").Element("v6");
                    XElement v7 = item.Element("goal").Element("v7");
                    newodds = neweventid + "@0" + "|";
                    if (v0 != null)
                    {
                        newodds = newodds + "0*" + v0.Value + "#";
                    }
                    if (v1 != null)
                    {
                        newodds = newodds + "1*" + v1.Value + "#";
                    }
                    if (v2 != null)
                    {
                        newodds = newodds + "2*" + v2.Value + "#";
                    }
                    if (v3 != null)
                    {
                        newodds = newodds + "3*" + v3.Value + "#";
                    }
                    if (v4 != null)
                    {
                        newodds = newodds + "4*" + v4.Value + "#";
                    }
                    if (v5 != null)
                    {
                        newodds = newodds + "5*" + v5.Value + "#";
                    }
                    if (v6 != null)
                    {
                        newodds = newodds + "6*" + v6.Value + "#";
                    }
                    if (v7 != null)
                    {
                        newodds = newodds + "7*" + v7.Value + "#";
                    }
                }

                /*竞彩篮球*/
                if (lotid == Convert.ToInt32(LotteryTypes.LcSf).ToString())
                {
                    XElement v3 = item.Element("vs").Element("v3");
                    XElement v0 = item.Element("vs").Element("v0");
                    newodds = neweventid + "@0" + "|";
                    if (v3 != null)
                    {
                        newodds = newodds + "3*" + v3.Value + "#";
                    }
                    if (v0 != null)
                    {
                        newodds = newodds + "0*" + v0.Value + "#";
                    }
                }
                if (lotid == Convert.ToInt32(LotteryTypes.LcRfsf).ToString())
                {
                    XElement let = item.Element("letVs").Element("letPoint");
                    XElement letv3 = item.Element("letVs").Element("v3");
                    XElement letv0 = item.Element("letVs").Element("v0");
                    newodds = neweventid + "@" + let.Value + "|";
                    if (letv3 != null)
                    {
                        newodds = newodds + "3*" + letv3.Value + "#";
                    }
                    if (letv0 != null)
                    {
                        newodds = newodds + "0*" + letv0.Value + "#";
                    }
                }
                if (lotid == Convert.ToInt32(LotteryTypes.LcSfc).ToString())
                {
                    XElement v01 = item.Element("diff").Element("v01");
                    XElement v02 = item.Element("diff").Element("v02");
                    XElement v03 = item.Element("diff").Element("v03");
                    XElement v04 = item.Element("diff").Element("v04");
                    XElement v05 = item.Element("diff").Element("v05");
                    XElement v06 = item.Element("diff").Element("v06");
                    XElement v11 = item.Element("diff").Element("v11");
                    XElement v12 = item.Element("diff").Element("v12");
                    XElement v13 = item.Element("diff").Element("v13");
                    XElement v14 = item.Element("diff").Element("v14");
                    XElement v15 = item.Element("diff").Element("v15");
                    XElement v16 = item.Element("diff").Element("v16");
                    newodds = neweventid + "@0" + "|";
                    if (v01 != null)
                    {
                        newodds = newodds + "01*" + v01.Value + "#";
                    }

                    if (v02 != null)
                    {
                        newodds = newodds + "02*" + v02.Value + "#";
                    }

                    if (v03 != null)
                    {
                        newodds = newodds + "03*" + v03.Value + "#";
                    }

                    if (v04 != null)
                    {
                        newodds = newodds + "04*" + v04.Value + "#";
                    }

                    if (v05 != null)
                    {
                        newodds = newodds + "05*" + v05.Value + "#";
                    }

                    if (v06 != null)
                    {
                        newodds = newodds + "06*" + v06.Value + "#";
                    }

                    if (v11 != null)
                    {
                        newodds = newodds + "11*" + v11.Value + "#";
                    }

                    if (v12 != null)
                    {
                        newodds = newodds + "12*" + v12.Value + "#";
                    }

                    if (v13 != null)
                    {
                        newodds = newodds + "13*" + v13.Value + "#";
                    }

                    if (v14 != null)
                    {
                        newodds = newodds + "14*" + v14.Value + "#";
                    }

                    if (v15 != null)
                    {
                        newodds = newodds + "15*" + v15.Value + "#";
                    }

                    if (v16 != null)
                    {
                        newodds = newodds + "16*" + v16.Value + "#";
                    }
                }


                if (lotid == Convert.ToInt32(LotteryTypes.LcDxf).ToString())
                {
                    XElement total = item.Element("bs").Element("basePoint");
                    XElement big = item.Element("bs").Element("g");
                    XElement small = item.Element("bs").Element("l");
                    if (total != null)
                    {
                        newodds = neweventid + "@" + total.Value + "|";
                    }
                    if (big != null)
                    {
                        newodds = newodds + "1*" + big.Value + "#";
                    }
                    if (small != null)
                    {
                        newodds = newodds + "2*" + small.Value + "#";
                    }
                }
                newcode = newcode + newodds + "^";
            }
            newcode = newcode.TrimEnd('^');
            return newcode;
        }
    }
}
