using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Security.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Liangcai.WebApi.Middlewares
{
    public class LiangcaiReceivingMiddleware
    {
        private RequestDelegate _next;
        private readonly ILogger<LiangcaiReceivingMiddleware> _logger;
        private readonly ILotteryNoticingMessagePublisher _lotteryNoticingMessagePublisher;
        private readonly IOrderingApplicationService _orderingApplicationService;
        private readonly ILotteryMerchanterApplicationService _lotteryMerchanterApplicationService;

        public LiangcaiReceivingMiddleware(RequestDelegate next, ILogger<LiangcaiReceivingMiddleware> logger, ILotteryNoticingMessagePublisher lotteryNoticingMessagePublisher, IOrderingApplicationService orderingApplicationService, ILotteryMerchanterApplicationService lotteryMerchanterApplicationService)
        {
            _next = next;
            _logger = logger;
            _lotteryNoticingMessagePublisher = lotteryNoticingMessagePublisher;
            _orderingApplicationService = orderingApplicationService;
            _lotteryMerchanterApplicationService = lotteryMerchanterApplicationService;
        }

        /// <summary>
        /// xAgent=3821&xAction=501&xSign=dfa62fc1b09d94fca897a3462a928545&xValue=D200900067_2003,D200900054_2003
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var xAgent = httpContext.Request.Form["xAgent"].ToString();
                var xAction = httpContext.Request.Form["xAction"].ToString();
                var xSign = httpContext.Request.Form["xSign"].ToString();
                var xValue = httpContext.Request.Form["xValue"].ToString();
                var merchanter = await _lotteryMerchanterApplicationService.FindMerchanter(xAgent);
                string str = $"{xAgent}{xAction}{xValue}{merchanter.SecretKey}";
                if (str.VerifyMd5(xSign))
                {
                    string[] items = xValue.Split(",");
                    foreach (var item in items)
                    {
                        string[] values = item.Split("_");
                        var order = await _orderingApplicationService.FindOrderAsync(values[0]);
                        if (order.Status < 4000)
                        {
                            LotteryTicketingTypes lotteryTicketingType = values[1] == "1" ? LotteryTicketingTypes.Success : LotteryTicketingTypes.Failure;
                            _logger.LogTrace($"{order.Id} Ticketed: {lotteryTicketingType}");
                            await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{xAgent}", new NoticeMessage<LotteryTicketed>(long.Parse(values[0]), xAgent, new LotteryTicketed
                            {
                                LvpMerchanerId = order.LdpVenderId,
                                LvpOrderId = order.LvpOrderId,
                                TicketingType = lotteryTicketingType,
                            }));
                        }
                        else
                        {
                            _logger.LogWarning($"{order.Id} {order.Status}");
                        }
                    }
                    await httpContext.Response.WriteAsync("1");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            await httpContext.Response.WriteAsync("0");
        }
    }
}
