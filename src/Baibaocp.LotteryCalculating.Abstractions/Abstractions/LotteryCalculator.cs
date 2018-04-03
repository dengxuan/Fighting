using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    public abstract class LotteryCalculator : ILotteryCalculator
    {
        protected IServiceProvider IocResolver { get; }

        protected LotteryMerchanteOrder LotteryMerchanteOrder { get; }

        public LotteryCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder)
        {
            IocResolver = iocResolver;
            LotteryMerchanteOrder = lotteryMerchanteOrder;
        }

        public abstract Task<Handle> CalculateAsync();
    }
}
