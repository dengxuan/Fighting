using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Core.Regions
{
    public class BbcpCityManager
    {
        private readonly IRepository<BbcpCity, int> _cityRepository;

        public virtual IQueryable<BbcpCity> Citys { get { return _cityRepository.GetAll(); } }

        public BbcpCityManager(IRepository<BbcpCity, int> cityRepository)
        {
            _cityRepository = cityRepository;
        }
    }
}
