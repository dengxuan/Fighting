using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities.Lotteries;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    /// <summary>
    /// 数字型彩票算奖
    /// </summary>
    public abstract class NumericLotteryCalculator : LotteryCalculator
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

        public NumericLotteryCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder) : base(iocResolver, lotteryMerchanteOrder)
        {
        }
    }
}
