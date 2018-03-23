using Baibaocp.LotteryCalculating.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating
{
    internal class LotteryCalculatorFactory : ILotteryCalculatorFactory
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ConcurrentDictionary<int, ILotteryCalculator> _lotteryCalculators = new ConcurrentDictionary<int, ILotteryCalculator>();

        private readonly IOrderingApplicationService _orderingApplicationService;

        public LotteryCalculatorFactory(IOrderingApplicationService orderingApplicationService)
        {
            _orderingApplicationService = orderingApplicationService;
        }
        public async Task<ILotteryCalculator> GetLotteryCalculatorAsync(long lotteryOrderId)
        {
            var lotteryMerchanteOrder = await _orderingApplicationService.FindOrderAsync(lotteryOrderId.ToString());
            ILotteryCalculator lotteryCalculator = null;
            _lotteryCalculators.GetOrAdd(lotteryMerchanteOrder.LotteryId, (lotteryId) => 
            {
                return null;
            });
            return lotteryCalculator;
        }
    }
}
