using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    public interface ILotteryCalculator
    {
        Task<Handle> CalculateAsync();
    }
}
