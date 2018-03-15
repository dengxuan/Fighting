using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Users
{

    [TransientDependency]
    public class UserLotteryBuyerOrderManager
    {
        private readonly IRepository<UserLotteryBuyerOrder, long> _orderRepository;

        public virtual IQueryable<UserLotteryBuyerOrder> Orders { get { return _orderRepository.GetAll(); } }

        public UserLotteryBuyerOrderManager(IRepository<UserLotteryBuyerOrder, long> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task UpdateOrderStatus(UserLotteryBuyerOrder bbcpOrder)
        {
            await _orderRepository.UpdateAsync(bbcpOrder);
        }
    }
}
