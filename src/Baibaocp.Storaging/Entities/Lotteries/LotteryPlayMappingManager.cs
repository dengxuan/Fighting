using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Foundation.Baibaocp.Lotteries
{

    [TransientDependency]
    public class LotteryPlayMappingManager
    {
        private readonly IRepository<LotteryPlayMapping, int> _lotteryPlayRepository;

        public virtual IQueryable<LotteryPlayMapping> LotteryPlayMappings { get { return _lotteryPlayRepository.GetAll(); } }

        public LotteryPlayMappingManager(IRepository<LotteryPlayMapping, int> lotteryPlayRepository)
        {
            _lotteryPlayRepository = lotteryPlayRepository;
        }
    }
}
