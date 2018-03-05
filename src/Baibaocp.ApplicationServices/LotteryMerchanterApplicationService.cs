using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities.Merchants;
using Fighting.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices
{
    public class LotteryMerchanterApplicationService : ILotteryMerchanterApplicationService
    {
        private readonly IIdentityGenerater _identityGenerater;

        private readonly MerchanterManager _merchanterManager;

        private readonly MerchanterAccountLoggingManager _merchanterAccountLoggingManager;

        public LotteryMerchanterApplicationService(IIdentityGenerater identityGenerater, MerchanterManager merchanterManager, MerchanterAccountLoggingManager merchanterAccountLoggingManager)
        {
            _identityGenerater = identityGenerater;
            _merchanterManager = merchanterManager;
            _merchanterAccountLoggingManager = merchanterAccountLoggingManager;
        }

        public async Task Recharging(int merchanterId, long orderId, int amount)
        {
            Merchanter merchanter = _merchanterManager.Merchanters.Where(predicate => predicate.Id == merchanterId).First();
            await _merchanterManager.IncreaseBalance(merchanter, amount);
            var merchanterAccountLogging = new MerchanterAccountLogging
            {
                Id = _identityGenerater.Generate(),
                MerchanterId = merchanterId,
                OrderId = orderId,
                OperationTypes = 1000,
                Amount = amount,
                Balance = merchanter.Balance
            };
            await _merchanterAccountLoggingManager.CreateAccountLogging(merchanterAccountLogging);
        }

        public async Task Rewarding(int merchanterId, long orderId, int amount)
        {
            Merchanter merchanter = _merchanterManager.Merchanters.Where(predicate => predicate.Id == merchanterId).First();
            await _merchanterManager.IncreaseAwardedAmount(merchanter, amount);
            var merchanterAccountLogging = new MerchanterAccountLogging
            {
                Id = _identityGenerater.Generate(),
                MerchanterId = merchanterId,
                OrderId = orderId,
                OperationTypes = 3000,
                Amount = amount,
                Balance = merchanter.Balance
            };
            await _merchanterAccountLoggingManager.CreateAccountLogging(merchanterAccountLogging);
        }

        public async Task Ticketing(int merchanterId, long orderId, int amount)
        {
            Merchanter merchanter = _merchanterManager.Merchanters.Where(predicate => predicate.Id == merchanterId).First();
            await _merchanterManager.IncreaseTicketedAmount(merchanter, amount);
            var merchanterAccountLogging = new MerchanterAccountLogging
            {
                Id = _identityGenerater.Generate(),
                MerchanterId = merchanterId,
                OrderId = orderId,
                OperationTypes = 2000,
                Amount = amount,
                Balance = merchanter.Balance
            };
            await _merchanterAccountLoggingManager.CreateAccountLogging(merchanterAccountLogging);
        }
    }
}
