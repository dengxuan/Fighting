using Baibaocp.Core.Lotteries;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;

namespace Baibaocp.Core.Foundation.Baibaocp.Lotteries
{
    public class BbcpLotteryPlayMappingManager
    {
        private readonly IRepository<BbcpLotteryPlayMapping, int> _lotteryPlayRepository;

        public virtual IQueryable<BbcpLotteryPlayMapping> LotteryPlayMappings { get { return _lotteryPlayRepository.GetAll(); } }

        public BbcpLotteryPlayMappingManager(IRepository<BbcpLotteryPlayMapping, int> lotteryPlayRepository)
        {
            _lotteryPlayRepository = lotteryPlayRepository;
        }
    }
}
