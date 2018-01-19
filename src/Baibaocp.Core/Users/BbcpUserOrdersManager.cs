using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Core.Users
{
    public class BbcpUserOrdersManager
    {
        private readonly IRepository<BbcpUserOrders, string> _orderRepository;

        public virtual IQueryable<BbcpUserOrders> Orders { get { return _orderRepository.GetAll(); } }

        public BbcpUserOrdersManager(IRepository<BbcpUserOrders, string> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task UpdateOrderStatus(BbcpUserOrders bbcpOrder)
        {
            await _orderRepository.UpdateAsync(bbcpOrder);
        }
    }
}
