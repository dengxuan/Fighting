using Fighting.DependencyInjection.Builder;
using Orleans;

namespace Fighting.ApplicationServices.Abstractions
{
    public interface IApplicationService : IGrainWithIntegerKey, ITransientDependency
    {
    }
}
