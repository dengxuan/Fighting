namespace Fighting.Orleans.Abstractions
{
    public interface IOrleansClusterManager<TCluster> where TCluster : IOrleansCluster
    {
        TCluster GetCluster();
    }
}
