using Fighting.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Orleans.ServerCluster
{
    internal class OrleansService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
