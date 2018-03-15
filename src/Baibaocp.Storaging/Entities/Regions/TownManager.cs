using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Regions
{

    [TransientDependency]
    public class TownManager
    {
        private readonly IRepository<Town, int> _townRepository;

        public virtual IQueryable<Town> Towns { get { return _townRepository.GetAll(); } }

        public TownManager(IRepository<Town, int> townRepository)
        {
            _townRepository = townRepository;
        }
    }
}
