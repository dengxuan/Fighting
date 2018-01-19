using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Core.Regions
{
    public class BbcpTownManager
    {
        private readonly IRepository<BbcpTown, int> _townRepository;

        public virtual IQueryable<BbcpTown> Towns { get { return _townRepository.GetAll(); } }

        public BbcpTownManager(IRepository<BbcpTown, int> townRepository)
        {
            _townRepository = townRepository;
        }
    }
}
