using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Fighting.Security.Cryptography;
using Fighting.Security.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;

namespace Baibaocp.LotteryDispatching.Liangcai.WebApi.Middlewares
{
    public class LiangcaiReceivingMiddleware
    {
        private RequestDelegate _next;
        private readonly string SecretKey = "ourpartner";
        private readonly ILogger<LiangcaiReceivingMiddleware> _logger;
        private readonly ILotteryNoticingMessagePublisher _lotteryNoticingMessagePublisher;
        private readonly IOrderingApplicationService _orderingApplicationService;

        public LiangcaiReceivingMiddleware(RequestDelegate next, ILogger<LiangcaiReceivingMiddleware> logger, ILotteryNoticingMessagePublisher lotteryNoticingMessagePublisher)
        {
            _next = next;
            _logger = logger;
            _lotteryNoticingMessagePublisher = lotteryNoticingMessagePublisher;
        }

        /// <summary>
        /// xAgent=3821&xAction=501&xSign=dfa62fc1b09d94fca897a3462a928545&xValue=D200900067_2003,D200900054_2003
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var merchanerId = httpContext.Request.Query["xAgent"];
            var command = httpContext.Request.Query["xAction"];
            var xSign = httpContext.Request.Query["xSign"];
            var xValue = httpContext.Request.Query["xValue"].ToString();
            string str = string.Format($"{0}{1}{2}{3}", merchanerId, command, xValue, SecretKey);
            if (str.VerifyMd5(xSign))
            {
                string[] items = xValue.Split(",");
                foreach (var item in items)
                {
                    string[] values = item.Split("_");
                    var order = await _orderingApplicationService.FindOrderAsync(values[0]);
                    LotteryTicketingTypes lotteryTicketingType = values[1] == "1" ? LotteryTicketingTypes.Success : LotteryTicketingTypes.Failure;
                    await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Ticketed.{merchanerId}", new NoticeMessage<LotteryTicketed>(long.Parse(values[0]), merchanerId, new LotteryTicketed
                    {
                        LvpMerchanerId = order.LdpVenderId,
                        LvpOrderId = order.LvpOrderId,
                        TicketingType = lotteryTicketingType,
                    }));
                }
                await httpContext.Response.WriteAsync("1");
            }
            await _next(httpContext);
        }
    }
}
