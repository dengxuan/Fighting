using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Users
{
    public class UserLotteryBuyerManager
    {
        private readonly IRepository<UserLotteryBuyer, long> _userLotteryBuyerRepository;

        public virtual IQueryable<UserLotteryBuyer> UserLotteryBuyers { get { return _userLotteryBuyerRepository.GetAll(); } }

        public UserLotteryBuyerManager(IRepository<UserLotteryBuyer, long> userLotteryBuyerRepository)
        {
            _userLotteryBuyerRepository = userLotteryBuyerRepository;
        }

        public async Task CreateBbcpUserLotteryBuyer(UserLotteryBuyer bbcpUserLotteryBuyer)
        {
            await _userLotteryBuyerRepository.InsertAsync(bbcpUserLotteryBuyer);
        }
    }
}
