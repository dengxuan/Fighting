using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    public class LotteryManager
    {

        private readonly IRepository<Lottery, int> _lotteryRepository;

        public virtual IQueryable<Lottery> Lotteries { get { return _lotteryRepository.GetAll(); } }

        public LotteryManager(IRepository<Lottery, int> lotteryRepository)
        {
            _lotteryRepository = lotteryRepository;
        }
    }
}
