using Baibaocp.LotteryNotifier.Abstractions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier
{
    internal class NoticeDispatcher : INoticeDispatcher
    {
        private readonly ILogger _logger;

        private readonly RetryPolicy<bool> _policy;

        private readonly LotteryNoticeOptions _options;

        private readonly INoticeHandlerFactory _handlerFactory;

        public NoticeDispatcher(LotteryNoticeOptions options, INoticeHandlerFactory handlerFactory, ILoggerFactory loggerFactory)
        {
            _options = options;
            _handlerFactory = handlerFactory;
            _logger = loggerFactory.CreateLogger<NoticeDispatcher>();
            _policy = Policy.Handle<Exception>().OrResult(false).WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning("推送失败:{0} {1} 重试中...", ex.Result, ex.Exception?.Message);
            });
        }

        public async Task<bool> DispatchAsync<TNotice>(INotifier<TNotice> notifier) where TNotice : class
        {
            try
            {
                bool result = await _policy.ExecuteAsync(async () =>
                {
                    NoticeConfiguration configuration = _options.Configures.Where(predicate => predicate.LvpVenderId == notifier.LvpVenderId).SingleOrDefault();
                    if (configuration == null)
                    {
                        return true;
                    }
                    var handler = _handlerFactory.GetHandler<TNotice>(configuration);

                    return await handler.HandleAsync(notifier.Notice);
                });
                _logger.LogWarning("Notice {0} result:{1}", notifier.LvpVenderId, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Notice error! {0}", notifier.LvpVenderId);
            }
            return true;
        }
    }
}
