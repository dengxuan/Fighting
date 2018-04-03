using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities.Lotteries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    /// <summary>
    /// 乐透型彩票
    /// </summary>
    public abstract class LottoLotteryCalculator : LotteryCalculator
    {
        protected async Task<LotteryPhase> FindLotteryPhaseAsync(int lotteryId, int issueNumber)
        {
            ILotteryPhaseApplicationService lotteryPhaseApplicationService = IocResolver.GetRequiredService<ILotteryPhaseApplicationService>();
            LotteryPhase lotteryPhase = await lotteryPhaseApplicationService.FindLotteryPhase(lotteryId, issueNumber);
            return lotteryPhase;
        }

        protected async Task<string> FindDrawNumberAsync(int lotteryId, int issueNumber)
        {
            LotteryPhase lotteryPhase = await FindLotteryPhaseAsync(lotteryId, issueNumber);
            return lotteryPhase.DrawNumber;
        }

        public LottoLotteryCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder) : base(iocResolver, lotteryMerchanteOrder)
        {
        }
    }
}
