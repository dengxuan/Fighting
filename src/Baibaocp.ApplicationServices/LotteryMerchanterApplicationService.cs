using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities.Merchants;
using Fighting.Abstractions;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.ApplicationServices
{
    public class LotteryMerchanterApplicationService : ApplicationService, ILotteryMerchanterApplicationService
    {

        private readonly MerchanterManager _merchanterManager;

        private readonly MerchanterLotteryMappingManager _merchanterLotteryMappingManager;

        private readonly MerchanterAccountLoggingManager _merchanterAccountLoggingManager;

        public LotteryMerchanterApplicationService(ICacheManager cacheManager, MerchanterManager merchanterManager, MerchanterAccountLoggingManager merchanterAccountLoggingManager, MerchanterLotteryMappingManager merchanterLotteryMappingManager) : base(cacheManager)
        {
            _merchanterManager = merchanterManager;
            _merchanterAccountLoggingManager = merchanterAccountLoggingManager;
            _merchanterLotteryMappingManager = merchanterLotteryMappingManager;
        }

        public async Task<Merchanter> FindMerchanterAsync(string merchanterId)
        {
            ICache cacher = CacheManager.GetCache("LotteryMerchanters");
            return await cacher.GetAsync(merchanterId, (key) =>
            {
                var merchanter = _merchanterManager.FindMerchanter(merchanterId);
                return merchanter;
            });
        }

        public async Task<string> FindLdpMerchanterIdAsync(string lvpMerchanterId, int lotteryId)
        {
            ICache cacher = CacheManager.GetCache("MerchanterLotteryMappings");
            IList<MerchanterLotteryMapping> merchanterLotteryMappings = await cacher.GetAsync($"{lvpMerchanterId}-{lotteryId}", (key) =>
            {
                return _merchanterLotteryMappingManager.FindLdpMerchanterId(lvpMerchanterId, lotteryId);
            });
            if (merchanterLotteryMappings.Count == 0)
            {
                return null;
            }
            Random r = new Random(DateTime.Now.Millisecond);
            int index = r.Next(0, merchanterLotteryMappings.Count);
            return merchanterLotteryMappings[index].LdpMerchanterId;
        }

        public async Task Recharging(string merchanterId, string orderId, int amount)
        {
            var isContains = await _merchanterAccountLoggingManager.IsContainsAsync(merchanterId, orderId, 1000);
            if (isContains == false)
            {
                Merchanter merchanter = await _merchanterManager.FindMerchanterAsync(merchanterId);
                await _merchanterManager.AddBalanceAsync(merchanter, amount);
                await _merchanterAccountLoggingManager.CreateAsync(merchanterId, orderId, amount, merchanter.Balance, 1000);
            }
        }

        public async Task Rewarding(string merchanterId, string orderId, int lotteryId, int amount)
        {
            var isContains = await _merchanterAccountLoggingManager.IsContainsAsync(merchanterId, orderId, 4000);
            if (isContains == false)
            {
                Merchanter merchanter = await _merchanterManager.FindMerchanterAsync(merchanterId);
                await _merchanterManager.AddBalanceAsync(merchanter, amount);
                await _merchanterManager.SubTotalAwardedAmount(merchanter, amount);
                await _merchanterAccountLoggingManager.CreateAsync(merchanterId, orderId, amount, merchanter.Balance, 4000, lotteryId);
            }
        }

        public async Task Ticketing(string merchanterId, string orderId, int lotteryId, int amount)
        {
            var isContains = await _merchanterAccountLoggingManager.IsContainsAsync(merchanterId, orderId, 3000);
            if (isContains == false)
            {
                Merchanter merchanter = await _merchanterManager.FindMerchanterAsync(merchanterId);
                await _merchanterManager.SubBalanceAsync(merchanter, amount);
                await _merchanterManager.AddTotalTicketedAmount(merchanter, amount);
                /* 出票流水计负数 */
                await _merchanterAccountLoggingManager.CreateAsync(merchanterId, orderId, 0 - amount, merchanter.Balance, 3000, lotteryId);
            }
        }
    }
}
