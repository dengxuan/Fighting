using Baibaocp.ApplicationServices;
using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.Storaging.Entities.Merchants;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier
{
    internal class TicketingNotifier : ITicketingNotifier
    {

        private readonly ConcurrentDictionary<string, Merchanter> _lotteryMerchanters = new ConcurrentDictionary<string, Merchanter>();

        private readonly ILogger<TicketingNotifier> _logger;

        private readonly RetryPolicy<bool> _policy;

        private readonly HttpClient _client;

        private readonly INoticeSerializer _serializer;

        private readonly ILotteryMerchanterApplicationService _lotteryMerchanterApplicationService;

        public TicketingNotifier(INoticeSerializer serializer, ILotteryMerchanterApplicationService lotteryMerchanterApplicationService, ILogger<TicketingNotifier> logger)
        {
            _serializer = serializer;
            _logger = logger;
            _lotteryMerchanterApplicationService = lotteryMerchanterApplicationService;
            _client = new HttpClient();

            _policy = Policy.Handle<Exception>().OrResult(false).WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning("推送失败:{0} {1} 重试中...", ex.Result, ex.Exception?.Message);
            });
        }

        public async Task<bool> DispatchAsync(LotteryTicketed message)
        {
            try
            {
                Merchanter merchanter = _lotteryMerchanters.GetOrAdd(message.LvpMerchanerId, (merchanterId) =>
                {
                    return _lotteryMerchanterApplicationService.FindMerchanterAsync(merchanterId).GetAwaiter().GetResult();
                });
                if(merchanter.IsNotice == false)
                {
                    return true;
                }
                return await _policy.ExecuteAsync(async () =>
                {
                    HttpResponseMessage responseMessage = (await _client.PostAsync(merchanter.TicketAddress, new ByteArrayContent(_serializer.Serialize(new { OrderId = message.LvpOrderId, TicketOdds = message.TicketedOdds, Status = (int)message.TicketingType })))).EnsureSuccessStatusCode();
                    byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
                    Handle result = _serializer.Deserialize<Handle>(bytes);
                    _logger.LogInformation("Notice {0} result:{1}", message.LvpOrderId, result);
                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Notice error! {0}", message.LvpOrderId);
            }
            return false;
        }
    }
}
