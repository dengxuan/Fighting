using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryCalculating.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Calculators
{
    public class KsCalculator : LotteryCalculator
    {

        private readonly ILotteryPhaseApplicationService _LotteryPhaseApplicationService;
        public KsCalculator(ILotteryPhaseApplicationService LotteryPhaseApplicationService, LotteryMerchanteOrder lotteryMerchanteOrder) : base(lotteryMerchanteOrder)
        {
            _LotteryPhaseApplicationService = LotteryPhaseApplicationService;
        }
        public override async Task<Handle> CalculateAsync()
        {
            decimal BonusMoney = 0;
            LotteryPhase lotteryPhase = await _LotteryPhaseApplicationService.FindLotteryPhase(LotteryMerchanteOrder.LotteryId, (int)LotteryMerchanteOrder.IssueNumber);
            string DrawNumber = lotteryPhase.DrawNumber;
            if (DrawNumber != "")
            {
                List<string> drawedNumbers = DrawNumber.Split(',').ToList();
                switch (LotteryMerchanteOrder.LotteryPlayId)
                {

                    case (int)PlayTypes.Ks_SumValue:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.SumValue(choseNumbers, drawedNumbers);
                        }
                        break;
                    case (int)PlayTypes.Ks_ThreeDiffSingle:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.ThreeDiffSingle(choseNumbers, drawedNumbers);
                        }
                        break;
                    case (int)PlayTypes.Ks_ThreeSameAll:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.ThreeSameAll(choseNumbers, drawedNumbers);
                        }
                        break;
                    case (int)PlayTypes.Ks_ThreeSameSingle:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.ThreeSameSingle(choseNumbers, drawedNumbers);
                        }
                        break;
                    case (int)PlayTypes.Ks_ThreeSeriesAll:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.ThreeLinkAll(choseNumbers, drawedNumbers);
                        }
                        break;
                    case (int)PlayTypes.Ks_TowDiffSingle:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.TowDiffSingle(choseNumbers, drawedNumbers);
                        }
                        break;
                    case (int)PlayTypes.Ks_TowSameAll:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.TowSameAll(choseNumbers, drawedNumbers);
                        }
                        break;
                    case (int)PlayTypes.Ks_TowSameSingle:
                        {
                            List<string> choseNumbers = LotteryMerchanteOrder.InvestCode.Split(',').ToList();
                            BonusMoney = this.TowSameSingle(choseNumbers, drawedNumbers);
                        }
                        break;
                }
                if (BonusMoney > 0)
                {
                    return Handle.Winner;
                }
                else
                {
                    return Handle.Losing;
                }
            }
            else {
                return Handle.Waiting;
            }
        }

        /// <summary>
        /// 两同号单选
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>奖金</returns>
        protected decimal TowSameSingle(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            chosedNumbers.Sort();
            drawedNumbers.Sort();
            for (int i = 0; i < 3; i++)
            {
                if (chosedNumbers[i] != drawedNumbers[i])
                {
                    return 0;
                }
            }
            decimal bonusAmount = 80;
            return bonusAmount;
        }

        /// <summary>
        /// 两同号通选(复选)
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>奖金</returns>
        protected decimal TowSameAll(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            for (int i = 0; i < chosedNumbers.Count; i++)
            {
                List<string> result = drawedNumbers.Where(match => match == chosedNumbers[i]).ToList();
                if (result.Count == 2 || result.Count == 3)
                {
                    decimal bonusAmount = 15;
                    return bonusAmount;
                }
            }
            return 0;
        }

        /// <summary>
        /// 两不同单式
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>奖金</returns>
        protected decimal TowDiffSingle(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            List<string> winnerNumbers = chosedNumbers.Intersect(drawedNumbers).ToList();
            if (winnerNumbers.Count == 2)
            {
                decimal bonusAmount = 8;
                return bonusAmount;
            }
            return 0;
        }

        /// <summary>
        /// 两不同胆拖
        /// </summary>
        /// <param name="fixedNumber">用户选择的胆码</param>
        /// <param name="chosedNumbers">用户选择的拖码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <param name="issueNumber">期号</param>
        /// <param name="bonusCount">中奖注数</param>
        /// <returns>奖金</returns>
        protected decimal TowDiffBraverTwo(string fixedNumber, List<string> chosedNumbers, List<string> drawedNumbers, int issueNumber, out int bonusCount)
        {
            Boolean fixedWinner = drawedNumbers.Contains(fixedNumber);
            bonusCount = 0;
            if (fixedWinner == false)
            {
                return 0;
            }
            List<string> chosedWinners = chosedNumbers.Intersect(drawedNumbers).ToList();
            bonusCount = chosedWinners.Count;
            decimal bonusAmount = 8;
            return bonusCount * bonusAmount;
        }

        /// <summary>
        /// 三同号单式
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>奖金</returns>
        protected decimal ThreeSameSingle(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            for (int i = 0; i < 3; i++)
            {
                if (chosedNumbers[i] != drawedNumbers[i])
                {
                    return 0;
                }
            }
            decimal bonusAmount = 240;
            return bonusAmount;
        }

        /// <summary>
        /// 三同号通选
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>奖金</returns>
        protected decimal ThreeSameAll(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            int count = drawedNumbers.Distinct().Count();
            if (count == 1)
            {
                decimal bonusAmount = 40;
                return bonusAmount;
            }
            return 0;
        }

        /// <summary>
        /// 三不同单选
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>奖金</returns>
        protected decimal ThreeDiffSingle(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            chosedNumbers.Sort();
            drawedNumbers.Sort();
            for (int i = 0; i < 3; i++)
            {
                if (chosedNumbers[i] != drawedNumbers[i])
                {
                    return 0;
                }
            }
            decimal bonusAmount = 40;
            return bonusAmount;
        }

        /// <summary>
        /// 三连号通选
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>奖金</returns>
        protected decimal ThreeLinkAll(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            drawedNumbers.Sort();
            string drawedNumberstring = string.Join(",", drawedNumbers);
            string[] numbers = new string[] { "1,2,3", "2,3,4", "3,4,5", "4,5,6" };
            if (numbers.Contains(drawedNumberstring))
            {
                decimal bonusAmount = 10;
                return bonusAmount;
            }
            return 0;
        }

        /// <summary>
        /// 和值
        /// </summary>
        /// <param name="chosedNumbers">用户选择的号码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <param name="issueNumber">期号</param>
        /// <param name="bonusCount">中奖注数</param>
        /// <returns>奖金</returns>
        protected decimal SumValue(List<string> chosedNumbers, List<string> drawedNumbers)
        {
            int sum = drawedNumbers.Sum(selector => Convert.ToInt32(selector));
            if (chosedNumbers.Contains(sum.ToString()))
            {
                decimal bonusAmount;
                switch (sum)
                {
                    case 4:
                        {
                            bonusAmount = 80;
                            break;
                        }
                    case 5:
                        {
                            bonusAmount = 40;
                            break;
                        }
                    case 6:
                        {
                            bonusAmount = 25;
                            break;
                        }
                    case 7:
                        {
                            bonusAmount = 16;
                            break;
                        }
                    case 8:
                        {
                            bonusAmount = 12;
                            break;
                        }
                    case 9:
                        {
                            bonusAmount = 10;
                            break;
                        }
                    case 10:
                        {
                            bonusAmount = 9;
                            break;
                        }
                    case 11:
                        {
                            bonusAmount = 9;
                            break;
                        }
                    case 12:
                        {
                            bonusAmount = 10;
                            break;
                        }
                    case 13:
                        {
                            bonusAmount = 12;
                            break;
                        }
                    case 14:
                        {
                            bonusAmount = 16;
                            break;
                        }
                    case 15:
                        {
                            bonusAmount = 25;
                            break;
                        }
                    case 16:
                        {
                            bonusAmount = 40;
                            break;
                        }
                    case 17:
                        {
                            bonusAmount = 80;
                            break;
                        }
                    default:
                        {
                            throw new Exception(string.Format("和值范围错误:{0}", sum));
                        }
                }
                return bonusAmount;
            }
            return 0;
        }
    }
}
