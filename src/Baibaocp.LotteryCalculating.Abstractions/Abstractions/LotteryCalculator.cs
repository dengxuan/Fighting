using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    public abstract class LotteryCalculator : ILotteryCalculator
    {
        protected LotteryMerchanteOrder LotteryMerchanteOrder { get; }

        public LotteryCalculator(LotteryMerchanteOrder lotteryMerchanteOrder)
        {
            LotteryMerchanteOrder = lotteryMerchanteOrder;
        }

        public abstract Task<Handle> CalculateAsync();
    }
}
