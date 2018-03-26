using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities.Merchants;
using Fighting.Abstractions;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices
{
    public class LotteryMerchanterApplicationService : ApplicationService, ILotteryMerchanterApplicationService
    {
        private readonly IIdentityGenerater _identityGenerater;

        private readonly MerchanterManager _merchanterManager;

        private readonly MerchanterAccountLoggingManager _merchanterAccountLoggingManager;

        private readonly Dictionary<string, Dictionary<int, string>> _merchanterMappings = new Dictionary<string, Dictionary<int, string>>
        {
            { "10081000345", new Dictionary<int, string> { { 1, "450022" }, { 2, "450022" }, { 31, "450022" }, { 20201, "800" }, { 20205, "800" }, { 20206, "800"} }},
        };

        public LotteryMerchanterApplicationService(ICacheManager cacheManager, IIdentityGenerater identityGenerater, MerchanterManager merchanterManager, MerchanterAccountLoggingManager merchanterAccountLoggingManager) : base(cacheManager)
        {
            _identityGenerater = identityGenerater;
            _merchanterManager = merchanterManager;
            _merchanterAccountLoggingManager = merchanterAccountLoggingManager;
        }

        public async Task<Merchanter> FindMerchanter(string merchanterId)
        {
            ICache cacher = CacheManager.GetCache("LotteryMerchanters");
            return await cacher.GetAsync(merchanterId, (key) =>
            {
                return _merchanterManager.Merchanters.FirstOrDefault(predicate => predicate.Id == merchanterId);
            });
        }

        public Task<string> FindLdpVenderId(string lvpVenderId, int lotteryId)
        {
            if (_merchanterMappings.TryGetValue(lvpVenderId, out Dictionary<int, string> lotteryMappings))
            {
                if (lotteryMappings.TryGetValue(lotteryId, out string ldpVenderId))
                {
                    return Task.FromResult(ldpVenderId);
                }
            }
            return Task.FromResult(string.Empty);
        }

        public async Task Recharging(string merchanterId, long orderId, int amount)
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

        public async Task Rewarding(string merchanterId, long orderId, int amount)
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

        public async Task Ticketing(string merchanterId, long orderId, int amount)
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
