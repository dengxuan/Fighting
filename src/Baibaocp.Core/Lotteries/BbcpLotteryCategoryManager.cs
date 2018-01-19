using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Core.Lotteries
{
    public class BbcpLotteryCategoryManager
    {

        protected readonly IRepository<BbcpLotteryCategory, int> _lotteryCategoriesRepository;

        public virtual IQueryable<BbcpLotteryCategory> LotteryCategories { get { return _lotteryCategoriesRepository.GetAll(); } }

        public BbcpLotteryCategoryManager(IRepository<BbcpLotteryCategory, int> lotteryCategoriesRepository)
        {
            _lotteryCategoriesRepository = lotteryCategoriesRepository;
        }
    }
}
