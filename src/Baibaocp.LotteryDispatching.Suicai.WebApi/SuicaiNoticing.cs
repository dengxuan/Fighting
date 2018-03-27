using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Fighting.Security.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Fighting.Security.Cryptography;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Fighting.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Baibaocp.LotteryDispatching.Suicai.WebApi
{
    public class SuicaiNoticing
    {
        private RequestDelegate _next;
        private readonly ILogger<SuicaiNoticing> _logger;
        private readonly ILotteryNoticingMessagePublisher _lotteryNoticingMessagePublisher;
        private readonly IOrderingApplicationService _orderingApplicationService;
        private readonly ILotteryMerchanterApplicationService _lotteryMerchanterApplicationService;
        protected Tripledescrypt _crypter;


        public SuicaiNoticing(RequestDelegate next, ILogger<SuicaiNoticing> logger, ILotteryNoticingMessagePublisher lotteryNoticingMessagePublisher, IOrderingApplicationService orderingApplicationService, ILotteryMerchanterApplicationService lotteryMerchanterApplicationService)
        {
            _next = next;
            _logger = logger;
            _lotteryNoticingMessagePublisher = lotteryNoticingMessagePublisher;
            _orderingApplicationService = orderingApplicationService;
            _lotteryMerchanterApplicationService = lotteryMerchanterApplicationService;
            _crypter = Tripledescrypt.Create(CipherMode.CBC, PaddingMode.PKCS7);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var result = string.Empty;
                
                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8))
                {
                    result = await reader.ReadToEndAsync();
                    ReqContent reqcon = JsonConvert.DeserializeObject<ReqContent>(result);
                    string apicode = reqcon.apiCode;
                    string partnerid = reqcon.partnerId;
                    string messageid = reqcon.messageId;
                    string hmac = reqcon.hmac;

                    ResContent rescon = new ResContent();
                    rescon.version = "1.0";
                    rescon.content = string.Empty;
                    rescon.partnerId = partnerid;
                    rescon.resCode = "1";
                    rescon.resMsg = "";
                    rescon.apiCode = apicode;
                    rescon.messageId = messageid;

                    string s = string.Format("{0}{1}{2}{3}", apicode, reqcon.content, messageid, partnerid);
                    var merchanter = await _lotteryMerchanterApplicationService.FindMerchanter(partnerid);
                    string sign = s.hmac_md5(merchanter.SecretKey.Substring(0, 16)).ToLower();
                    if (sign == reqcon.hmac)
                    {
                        string CipherText = _crypter.Decrypt(reqcon.content, merchanter.SecretKey);
                        if (apicode == "300002")
                        {
                            if (await TicketNoticing(CipherText))
                            {
                                rescon.resCode = "0";
                            }
                        }
                    }
                    else {
                        rescon.resMsg = "签名错误";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            await httpContext.Response.WriteAsync("0");
        }

        private async Task<bool> TicketNoticing(string result)
        {
            JObject jarr = JObject.Parse(result);
            if (jarr.HasValues)
            {
                foreach (var json in jarr["notifyList"])
                {
                    string Status = json["status"].ToString();
                    string orderid = json["orderId"].ToString();
                    var order = await _orderingApplicationService.FindOrderAsync(orderid);
                    if (order.Status == 2000)
                    {

                        if (Status.IsIn("0", "1"))
                        {
                            return false;
                        }
                        else if (Status.Equals("2"))
                        {
                            string ticketId = json["tickSn"].ToString();
                            DateTime tickettime = DateTime.Now;
                            LotteryTicketingTypes lotteryTicketingType = LotteryTicketingTypes.Success;
                            await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{order.LdpVenderId}", new NoticeMessage<LotteryTicketed>(long.Parse(orderid), order.LdpVenderId, new LotteryTicketed
                            {
                                LvpMerchanerId = order.LdpVenderId,
                                LvpOrderId = order.LvpOrderId,
                                TicketingType = lotteryTicketingType,
                            }));
                            return true;
                        }
                        else
                        {
                            LotteryTicketingTypes lotteryTicketingType = LotteryTicketingTypes.Success;
                            await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{order.LdpVenderId}", new NoticeMessage<LotteryTicketed>(long.Parse(orderid), order.LdpVenderId, new LotteryTicketed
                            {
                                LvpMerchanerId = order.LdpVenderId,
                                LvpOrderId = order.LvpOrderId,
                                TicketingType = lotteryTicketingType,
                            }));
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        private async Task<bool> AwardNoticing(string result)
        {
            JObject jarr = JObject.Parse(result);
            if (jarr.HasValues)
            {
                foreach (var json in jarr["notifyList"])
                {
                    string Status = json["status"].ToString();
                    string orderid = json["orderId"].ToString();
                    var order = await _orderingApplicationService.FindOrderAsync(orderid);
                    if (order.Status == 2000)
                    {

                        if (Status.IsIn("0", "1"))
                        {
                            return false;
                        }
                        else if (Status.Equals("2"))
                        {
                            string ticketId = json["tickSn"].ToString();
                            DateTime tickettime = DateTime.Now;
                            LotteryTicketingTypes lotteryTicketingType = LotteryTicketingTypes.Success;
                            await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{order.LdpVenderId}", new NoticeMessage<LotteryTicketed>(long.Parse(orderid), order.LdpVenderId, new LotteryTicketed
                            {
                                LvpMerchanerId = order.LdpVenderId,
                                LvpOrderId = order.LvpOrderId,
                                TicketingType = lotteryTicketingType,
                            }));
                            return true;
                        }
                        else
                        {
                            LotteryTicketingTypes lotteryTicketingType = LotteryTicketingTypes.Success;
                            await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{order.LdpVenderId}", new NoticeMessage<LotteryTicketed>(long.Parse(orderid), order.LdpVenderId, new LotteryTicketed
                            {
                                LvpMerchanerId = order.LdpVenderId,
                                LvpOrderId = order.LvpOrderId,
                                TicketingType = lotteryTicketingType,
                            }));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
