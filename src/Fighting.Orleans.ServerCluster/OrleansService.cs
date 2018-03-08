using Fighting.Hosting;
using Fighting.Orleans.Abstractions;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Orleans.ServerCluster
{
    internal class OrleansService<TCluster> : BackgroundService where TCluster : IOrleansCluster
    {
        private readonly OrleansOptions _orleansOptions;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .Configure(options => options.ClusterId = _orleansOptions.ClusterId)
                .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(_orleansOptions.ClusterAddress, _orleansOptions.ClusterPort))
                .ConfigureEndpoints(_orleansOptions.ClusterAddress, _orleansOptions.ClusterPort, _orleansOptions.GetwayPort)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TCluster).Assembly).WithReferences());
            //.ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            return host.StartAsync();
        }
    }
}
