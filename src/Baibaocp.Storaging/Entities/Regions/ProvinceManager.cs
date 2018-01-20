using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Regions
{
    public class ProvinceManager
    {
        private readonly IRepository<Province, int> _provinceRepository;

        public virtual IQueryable<Province> Provinces { get { return _provinceRepository.GetAll(); } }

        public ProvinceManager(IRepository<Province, int> provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }
    }
}
