using Fighting.ApplicationServices.Abstractions;
using Orleans;

namespace Fighting.ApplicationServices
{
    public class ApplicationServiceCluster: IApplicationServiceCluster
    {
        private readonly IGrainFactory _factory;

        public ApplicationServiceCluster(IGrainFactory factory)
        {
            _factory = factory;
        }

        public TApplicationService GetApplicationService<TApplicationService>() where TApplicationService : IApplicationService
        {
            return _factory.GetGrain<TApplicationService>(0);
        }
    }
}
