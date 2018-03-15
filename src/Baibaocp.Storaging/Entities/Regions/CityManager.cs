using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Regions
{

    [TransientDependency]
    public class CityManager
    {
        private readonly IRepository<City, int> _cityRepository;

        public virtual IQueryable<City> Citys { get { return _cityRepository.GetAll(); } }

        public CityManager(IRepository<City, int> cityRepository)
        {
            _cityRepository = cityRepository;
        }
    }
}
