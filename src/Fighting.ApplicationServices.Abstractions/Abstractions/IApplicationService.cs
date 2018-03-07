using Fighting.DependencyInjection.Builder;
using Fighting.Orleans.Abstractions;

namespace Fighting.ApplicationServices.Abstractions
{
    [TransientDependency]
    public interface IApplicationService : IOrleansCluster
    {
    }
}
