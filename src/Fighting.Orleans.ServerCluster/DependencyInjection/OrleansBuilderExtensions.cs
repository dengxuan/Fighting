using Fighting.Orleans.Abstractions;
using Fighting.Orleans.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Runtime;
using System.Net;
using System.Threading.Tasks;

namespace Fighting.Orleans.ServerCluster.DependencyInjection
{
    public static class OrleansBuilderExtensions
    {
        public static OrleansBuilder UseClientCluster<TOrleansCluster>(this OrleansBuilder orleansBuilder, OrleansOptions orleansOptions) where TOrleansCluster : IOrleansCluster
        {
            orleansBuilder.Services.AddSingleton<IHostedService, OrleansService>();
            orleansBuilder.Services.AddSingleton<Task<IGrainFactory>>(async sp =>
            {
                var client = new ClientBuilder().ConfigureCluster(options => options.ClusterId = orleansOptions.ClusterId)
                                                .UseStaticClustering(options => options.Gateways.Add((new IPEndPoint(orleansOptions.ClusterAddress, orleansOptions.ClusterPort)).ToGatewayUri()))
                                                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TOrleansCluster).Assembly).WithReferences())
                                                .Build();

                await client.Connect();
                return client;
            });
            return orleansBuilder;
        }
    }
}
