using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Core.Regions
{
    public class BbcpProvinceManager
    {
        private readonly IRepository<BbcpProvince, int> _provinceRepository;

        public virtual IQueryable<BbcpProvince> Provinces { get { return _provinceRepository.GetAll(); } }

        public BbcpProvinceManager(IRepository<BbcpProvince, int> provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }
    }
}
