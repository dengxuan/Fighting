using Fighting.Orleans.Abstractions;
using Fighting.Orleans.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fighting.Orleans.ServerCluster.DependencyInjection
{
    public static class OrleansBuilderExtensions
    {
        public static OrleansBuilder UseServerCluster<TOrleansCluster>(this OrleansBuilder orleansBuilder, OrleansOptions orleansOptions) where TOrleansCluster : IOrleansCluster
        {
            orleansBuilder.Services.AddSingleton<IHostedService, OrleansService<IOrleansCluster>>();
            return orleansBuilder;
        }
    }
}
