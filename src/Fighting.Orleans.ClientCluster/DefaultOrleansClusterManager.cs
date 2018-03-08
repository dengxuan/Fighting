using Fighting.Orleans.Abstractions;

namespace Fighting.Orleans.ClientCluster
{
    public class DefaultOrleansClusterManager<TCluster> : IOrleansClusterManager<TCluster> where TCluster : IOrleansCluster
    {
        private readonly ClusterFactory<TCluster> _factory;

        public DefaultOrleansClusterManager(ClusterFactory<TCluster> factory)
        {
            _factory = factory;
        }

        public TCluster GetCluster()
        {
            return _factory.ClusterResolver.GetGrain<TCluster>(0);
        }
    }
}
