using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Core.Lotteries
{
    public class BbcpLotteryManager
    {

        private readonly IRepository<BbcpLottery, int> _lotteryRepository;

        public virtual IQueryable<BbcpLottery> Lotteries { get { return _lotteryRepository.GetAll(); } }

        public BbcpLotteryManager(IRepository<BbcpLottery, int> lotteryRepository)
        {
            _lotteryRepository = lotteryRepository;
        }
    }
}
