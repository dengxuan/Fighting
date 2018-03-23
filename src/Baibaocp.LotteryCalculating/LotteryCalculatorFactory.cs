using Baibaocp.LotteryCalculating.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using System.Collections.Generic;
using Baibaocp.LotteryCalculating.Calculators;
using Baibaocp.Storaging.Entities.Lotteries;
using Baibaocp.Storaging.Entities;

namespace Baibaocp.LotteryCalculating
{
    internal class LotteryCalculatorFactory : ILotteryCalculatorFactory
    {

        private readonly ConcurrentDictionary<int, ObjectFactory> _objectFactories = new ConcurrentDictionary<int, ObjectFactory>();

        private readonly IDictionary<int, Type> _lotteryCalculatorImplementationTypes = new Dictionary<int, Type>
        {
            /* 竞彩足球 */
            { 20201 , typeof(FootballCalculator) },
            { 20202 , typeof(FootballCalculator) },
            { 20203 , typeof(FootballCalculator) },
            { 20204 , typeof(FootballCalculator) },
            { 20205 , typeof(FootballCalculator) },
            { 20206 , typeof(FootballCalculator) }
        };

        private readonly IServiceProvider _iocResolver;

        private readonly IOrderingApplicationService _orderingApplicationService;

        public LotteryCalculatorFactory(IOrderingApplicationService orderingApplicationService)
        {
            _orderingApplicationService = orderingApplicationService;
        }

        public async Task<ILotteryCalculator> GetLotteryCalculatorAsync(long lotteryOrderId)
        {
            var lotteryMerchanteOrder = await _orderingApplicationService.FindOrderAsync(lotteryOrderId.ToString());

            ObjectFactory objectFactory = _objectFactories.GetOrAdd(lotteryMerchanteOrder.LotteryId, (lotteryId) =>
            {
                if (_lotteryCalculatorImplementationTypes.TryGetValue(lotteryId, out Type implementationType))
                {

                    ObjectFactory factory = ActivatorUtilities.CreateFactory(implementationType, new Type[] { typeof(LotteryMerchanteOrder) });
                    return factory;
                }
                throw new KeyNotFoundException($"Lottery {nameof(lotteryId)} not implementation ILotteryCalculator");
            });

            ILotteryCalculator lotteryCalculator = objectFactory.Invoke(_iocResolver, new[] { lotteryMerchanteOrder }) as ILotteryCalculator;

            return lotteryCalculator;
        }
    }
}
