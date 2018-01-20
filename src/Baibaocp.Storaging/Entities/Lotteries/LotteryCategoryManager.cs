using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    public class LotteryCategoryManager
    {

        protected readonly IRepository<LotteryCategory, short> _lotteryCategoriesRepository;

        public virtual IQueryable<LotteryCategory> LotteryCategories { get { return _lotteryCategoriesRepository.GetAll(); } }

        public LotteryCategoryManager(IRepository<LotteryCategory, short> lotteryCategoriesRepository)
        {
            _lotteryCategoriesRepository = lotteryCategoriesRepository;
        }
    }
}
