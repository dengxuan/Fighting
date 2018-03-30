using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Fighting.DependencyInjection.Builder;
using Fighting.Abstractions;

namespace Baibaocp.Storaging.Entities.Merchants
{
    [TransientDependency]
    public class MerchanterAccountLoggingManager
    {
        private readonly IIdentityGenerater _identityGenerater;

        private readonly IRepository<MerchanterAccountLogging, long> _tradeLoggingRepositiry;

        public virtual IQueryable<MerchanterAccountLogging> TradeLoggings { get { return _tradeLoggingRepositiry.GetAll(); } }

        public MerchanterAccountLoggingManager(IIdentityGenerater identityGenerater, IRepository<MerchanterAccountLogging, long> tradeLoggingRepositiry)
        {
            _identityGenerater = identityGenerater;
            _tradeLoggingRepositiry = tradeLoggingRepositiry;
        }

        public Task<bool> IsContainsAsync(string merchanterId, string orderId)
        {
            var isConsains = TradeLoggings.Where(predicate => predicate.MerchanterId == merchanterId)
                                          .Where(predicate => predicate.OrderId == orderId)
                                          .Any();
            return Task.FromResult(isConsains);
        }

        public async Task CreateAsync(string merchanterId, string orderId, int amount, int balance, int operationType, int? lotteryId = null)
        {
            MerchanterAccountLogging tradeLogging = new MerchanterAccountLogging
            {
                Id = _identityGenerater.Generate(),
                MerchanterId = merchanterId,
                OrderId = orderId,
                LotteryId = lotteryId ?? 0,
                Amount = amount,
                Balance = balance,
                OperationTypes = operationType
            };
            await _tradeLoggingRepositiry.InsertAsync(tradeLogging);
        }
    }
}
