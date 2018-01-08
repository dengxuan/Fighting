using Fighting.Hosting;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.LiangcaiHosting
{
    internal class LiangcaiLotteryDispatcherService : BackgroundService
    {
        private readonly AutoResetEvent _waitHandler = new AutoResetEvent(false);

        private readonly JobStorage _jobStorage;

        private readonly IEnumerable<IBackgroundProcess> _additionalProcesses;

        private readonly NodeConfiguration _configuration;

        private readonly ILogger<LiangcaiLotteryDispatcherService> _logger;

        private readonly IApplicationLifetime _applicationLifetime;

        public LiangcaiLotteryDispatcherService(NodeConfiguration configuration, ILogger<LiangcaiLotteryDispatcherService> logger, JobStorage jobStorage, IEnumerable<IBackgroundProcess> additionalProcesses, IApplicationLifetime applicationLifetime)
        {
            _configuration = configuration;
            _logger = logger;
            _jobStorage = jobStorage;
            _additionalProcesses = additionalProcesses;
            _applicationLifetime = applicationLifetime;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new BackgroundJobServerOptions
            {
                ServerName = _configuration.Identifier,
                WorkerCount = _configuration.WorkerCount,
                Queues = _configuration.Queues.Split(',')
            };
            _logger.LogInformation("The hangfire server {0} [queues: {1}, workercount: {2}] is now running.", _configuration.Identifier, _configuration.Queues, _configuration.WorkerCount);
            var server = new BackgroundJobServer(options, _jobStorage, _additionalProcesses);

            _applicationLifetime.ApplicationStopping.Register(() => server.SendStop());
            _applicationLifetime.ApplicationStopped.Register(() => server.Dispose());
            return Task.CompletedTask;
        }
    }
}
