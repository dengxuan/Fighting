using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities.Merchants;
using Fighting.Abstractions;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices
{
    public class LotteryMerchanterApplicationService : ApplicationService, ILotteryMerchanterApplicationService
    {
        private readonly IIdentityGenerater _identityGenerater;

        private readonly MerchanterManager _merchanterManager;

        private readonly MerchanterLotteryMappingManager _merchanterLotteryMappingManager;

        private readonly MerchanterAccountLoggingManager _merchanterAccountLoggingManager;

        public LotteryMerchanterApplicationService(ICacheManager cacheManager, IIdentityGenerater identityGenerater, MerchanterManager merchanterManager, MerchanterAccountLoggingManager merchanterAccountLoggingManager, MerchanterLotteryMappingManager merchanterLotteryMappingManager) : base(cacheManager)
        {
            _identityGenerater = identityGenerater;
            _merchanterManager = merchanterManager;
            _merchanterAccountLoggingManager = merchanterAccountLoggingManager;
            _merchanterLotteryMappingManager = merchanterLotteryMappingManager;
        }

        public async Task<Merchanter> FindMerchanter(string merchanterId)
        {
            ICache cacher = CacheManager.GetCache("LotteryMerchanters");
            return await cacher.GetAsync(merchanterId, (key) =>
            {
                return _merchanterManager.Merchanters.FirstOrDefault(predicate => predicate.Id == merchanterId);
            });
        }

        public async Task<string> FindLdpMerchanterId(string lvpMerchanterId, int lotteryId)
        {
            ICache cacher = CacheManager.GetCache("MerchanterLotteryMappings");
            IList<MerchanterLotteryMapping> merchanterLotteryMappings = await cacher.GetAsync($"{lvpMerchanterId}-{lotteryId}", (key) =>
            {
                return _merchanterLotteryMappingManager.FindLdpMerchanterId(lvpMerchanterId, lotteryId);
            });
            if(merchanterLotteryMappings.Count == 0)
            {
                return null;
            }
            Random r = new Random(DateTime.Now.Millisecond);
            int index = r.Next(0, merchanterLotteryMappings.Count);
            return merchanterLotteryMappings[index].LdpMerchanterId;
        }

        public async Task Recharging(string merchanterId, string orderId, int amount)
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

        public async Task Rewarding(string merchanterId, string orderId, int lotteryId, int amount)
        {
            Merchanter merchanter = _merchanterManager.Merchanters.Where(predicate => predicate.Id == merchanterId).First();
            await _merchanterManager.IncreaseAwardedAmount(merchanter, amount);
            await _merchanterManager.IncreaseBalance(merchanter, amount);
            var merchanterAccountLogging = new MerchanterAccountLogging
            {
                Id = _identityGenerater.Generate(),
                MerchanterId = merchanterId,
                OrderId = orderId,
                OperationTypes = 4000,
                LotteryId = lotteryId,
                Amount = amount,
                Balance = merchanter.Balance
            };
            await _merchanterAccountLoggingManager.CreateAccountLogging(merchanterAccountLogging);
        }

        public async Task Ticketing(string merchanterId, string orderId, int lotteryId, int amount)
        {
            Merchanter merchanter = _merchanterManager.Merchanters.Where(predicate => predicate.Id == merchanterId).First();
            await _merchanterManager.DecreaseBalance(merchanter, amount);
            await _merchanterManager.IncreaseTicketedAmount(merchanter, amount);
            var merchanterAccountLogging = new MerchanterAccountLogging
            {
                Id = _identityGenerater.Generate(),
                MerchanterId = merchanterId,
                OrderId = orderId,
                OperationTypes = 3000,
                LotteryId = lotteryId,
                Amount = amount,
                Balance = merchanter.Balance
            };
            await _merchanterAccountLoggingManager.CreateAccountLogging(merchanterAccountLogging);
        }
    }
}
