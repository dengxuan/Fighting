using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Core.Lotteries
{
    public class BbcpLotteryTypeManager
    {
        private readonly IRepository<BbcpLotteryType, int> _lotteryTypeRepository;

        public virtual IQueryable<BbcpLotteryType> LotteryTypes { get { return _lotteryTypeRepository.GetAll(); } }

        public BbcpLotteryTypeManager(IRepository<BbcpLotteryType, int> lotteryTypeRepository)
        {
            _lotteryTypeRepository = lotteryTypeRepository;
        }
    }
}
