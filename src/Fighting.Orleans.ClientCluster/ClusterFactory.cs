using Fighting.Orleans.Abstractions;
using Orleans;

namespace Fighting.Orleans.ClientCluster
{
    public class ClusterFactory<TOrleansCluster> where TOrleansCluster : IOrleansCluster
    {
        public IGrainFactory ClusterResolver { get; set; }
    }
}
