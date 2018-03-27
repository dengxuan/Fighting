using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier
{
    internal class TicketingNotifier : ITicketingNotifier
    {
        private readonly ILogger<TicketingNotifier> _logger;

        private readonly RetryPolicy<bool> _policy;

        private readonly LotteryNoticeOptions _options;

        private readonly HttpClient _client;

        private readonly INoticeSerializer _serializer;

        public TicketingNotifier(LotteryNoticeOptions options, INoticeSerializer serializer, ILogger<TicketingNotifier> logger)
        {
            _options = options;
            _serializer = serializer;
            _logger = logger;

            _client = new HttpClient
            {
                BaseAddress = new Uri(_options.Configuration.TicketedUrl)
            };

            _policy = Policy.Handle<Exception>().OrResult(false).WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning("推送失败:{0} {1} 重试中...", ex.Result, ex.Exception?.Message);
            });
        }

        public async Task<bool> DispatchAsync(LotteryTicketed message)
        {
            try
            {
                return await _policy.ExecuteAsync(async () =>
                {
                    HttpResponseMessage responseMessage = (await _client.PostAsync(_options.Configuration.TicketedUrl, new ByteArrayContent(_serializer.Serialize(new { OrderId = message.LvpOrderId, TicketOdds = message.TicketedOdds, Status = (int)message.TicketingType })))).EnsureSuccessStatusCode();
                    byte[] bytes = await responseMessage.Content.ReadAsByteArrayAsync();
                    Handle result = _serializer.Deserialize<Handle>(bytes);
                    _logger.LogInformation("Notice {0} result:{1}", message.LvpOrderId, result);
                    return result.Code == 0;
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
