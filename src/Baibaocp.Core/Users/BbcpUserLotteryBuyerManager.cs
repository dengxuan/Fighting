using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Core.Users
{
    public class BbcpUserLotteryBuyerManager
    {
        private readonly IRepository<BbcpUserLotteryBuyer, long> _userLotteryBuyerRepository;

        public virtual IQueryable<BbcpUserLotteryBuyer> UserLotteryBuyers { get { return _userLotteryBuyerRepository.GetAll(); } }

        public BbcpUserLotteryBuyerManager(IRepository<BbcpUserLotteryBuyer, long> userLotteryBuyerRepository)
        {
            _userLotteryBuyerRepository = userLotteryBuyerRepository;
        }

        public async Task CreateBbcpUserLotteryBuyer(BbcpUserLotteryBuyer bbcpUserLotteryBuyer)
        {
            await _userLotteryBuyerRepository.InsertAsync(bbcpUserLotteryBuyer);
        }
    }
}
