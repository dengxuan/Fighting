using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    public interface ILotteryCalculatorFactory
    {
        Task<ILotteryCalculator> GetLotteryCalculatorAsync(long lotteryOrderId);
    }
}
