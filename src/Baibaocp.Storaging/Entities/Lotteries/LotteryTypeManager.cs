using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Lotteries
{

    [TransientDependency]
    public class LotteryTypeManager
    {
        private readonly IRepository<LotteryType, short> _lotteryTypeRepository;

        public virtual IQueryable<LotteryType> LotteryTypes { get { return _lotteryTypeRepository.GetAll(); } }

        public LotteryTypeManager(IRepository<LotteryType, short> lotteryTypeRepository)
        {
            _lotteryTypeRepository = lotteryTypeRepository;
        }
    }
}
