using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryCalculating.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Calculators
{
    public class SyxwCalculator : LottoLotteryCalculator
    {
        public SyxwCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder) : base(iocResolver, lotteryMerchanteOrder)
        {
        }

        public override async Task<Handle> CalculateAsync()
        {
            int level = 0;
            string drawNumber = await FindDrawNumberAsync(LotteryMerchanteOrder.LotteryId, LotteryMerchanteOrder.IssueNumber.Value);
            if (string.IsNullOrEmpty(drawNumber))
            {
                return Handle.Waiting;
            }
            switch (LotteryMerchanteOrder.LotteryPlayId)
            {
                case (int)PlayTypes.Syxw_FrontTowFixedPositionSingle:
                    level = FrontMultiple(LotteryMerchanteOrder.InvestCode, drawNumber, 2);
                    break;
                case (int)PlayTypes.Syxw_FrontTowAnyPositionSingle:
                    level = AnyMultiple(LotteryMerchanteOrder.InvestCode, drawNumber, 2);
                    break;
                case (int)PlayTypes.Syxw_FrontTowAnyPositionFixedUnset:
                    level = FrontAnyFixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 2);
                    break;
                case (int)PlayTypes.Syxw_FrontThreeFixedPositionSingle:
                    level = FrontMultiple(LotteryMerchanteOrder.InvestCode, drawNumber, 3);
                    break;
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionSingle:
                    level = AnyMultiple(LotteryMerchanteOrder.InvestCode, drawNumber, 3);
                    break;
                case (int)PlayTypes.Syxw_FrontThreeAnyPositionFixedUnset:
                    level = FrontAnyFixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 3);
                    break;
                case (int)PlayTypes.Syxw_FrontOneSingle:
                    level = FrontMultiple(LotteryMerchanteOrder.InvestCode, drawNumber, 1);
                    break;
                case (int)PlayTypes.Syxw_AnyTowSingle:
                    level = Multiple(LotteryMerchanteOrder.InvestCode, drawNumber, 2);
                    break;
                case (int)PlayTypes.Syxw_AnyTowFixedUnset:
                    level = FixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 2);
                    break;
                case (int)PlayTypes.Syxw_AnyThreeSingle:
                    level = Multiple(LotteryMerchanteOrder.InvestCode, drawNumber, 3);
                    break;
                case (int)PlayTypes.Syxw_AnyThreeFixedUnset:
                    level = FixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 3);
                    break;
                case (int)PlayTypes.Syxw_AnySixSingle:
                    level = Multiple(LotteryMerchanteOrder.InvestCode, drawNumber, 6);
                    break;
                case (int)PlayTypes.Syxw_AnySixFixedUnset:
                    level = FixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 6);
                    break;
                case (int)PlayTypes.Syxw_AnySevenSingle:
                    level = Multiple(LotteryMerchanteOrder.InvestCode, drawNumber, 7);
                    break;
                case (int)PlayTypes.Syxw_AnySevenFixedUnset:
                    level = FixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 7);
                    break;
                case (int)PlayTypes.Syxw_AnyFourSingle:
                    level = Multiple(LotteryMerchanteOrder.InvestCode, drawNumber, 4);
                    break;
                case (int)PlayTypes.Syxw_AnyFourFixedUnset:
                    level = FixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 4);
                    break;
                case (int)PlayTypes.Syxw_AnyFiveSingle:
                    level = Multiple(LotteryMerchanteOrder.InvestCode, drawNumber, 5);
                    break;
                case (int)PlayTypes.Syxw_AnyFiveFixedUnset:
                    level = FixedUnset(LotteryMerchanteOrder.InvestCode, drawNumber, 5);
                    break;
                case (int)PlayTypes.Syxw_AnyEightSingle:
                    level = Single(LotteryMerchanteOrder.InvestCode, drawNumber, 8);
                    break;
            }
            if (level > 0)
            {
                return Handle.Winner;
            }
            else
            {
                return Handle.Losing;
            }
        }

        public int Single(string code, string drawedNumber, int size)
        {
            if (size < 5)
                return code.Split(',').Intersect(drawedNumber.Split(',')).Count() == size ? 1 : 0;
            else
                return code.Split(',').Intersect(drawedNumber.Split(',')).Count() == 5 ? 1 : 0;
        }

        public int AnySingle(string code, string drawedNumber, int size)
        {
            var codes = code.Split(',');
            var draweds = drawedNumber.Split(',').ToList();
            draweds.RemoveRange(codes.Length, size - codes.Length);
            return draweds.Intersect(codes).Count() == size ? 1 : 0;
        }

        public int FrontSingle(string code, string drawedNumber, int size)
        {
            string[] codes = code.Split(',');
            string[] draweds = drawedNumber.Split(',');
            for (int i = 0; i < codes.Length; i++)
            {
                if (codes[i] != draweds[i])
                    return 0;
            }
            return 1;
        }

        public int Multiple(string code, string drawedNumber, int size)
        {
            string[] codes = code.Split(',');
            string[] draweds = drawedNumber.Split(',');
            var list = codes.Intersect(draweds);
            if (size < 5)
            {
                if (list.Count() >= size)
                    return 1;
            }
            else
            {
                if (list.Count() == 5)
                    return 1;
            }
            return 0;
        }

        public int AnyMultiple(string code, string drawedNumber, int size)
        {
            var codes = code.Split(',');
            var draweds = drawedNumber.Split(',').ToList();
            draweds.RemoveRange(size, draweds.Count - size);
            if (size < 5)
                return draweds.Intersect(codes).Count() >= size ? 1 : 0;
            else
                return draweds.Intersect(codes).Count() == 5 ? 1 : 0;
        }

        public int FrontMultiple(string code, string drawedNumber, int size)
        {
            string[] positionCodes = code.Split('*');
            string[] draweds = drawedNumber.Split(',');
            int drawed = 0;
            for (int i = 0; i < positionCodes.Length; i++)
            {
                string[] codes = positionCodes[i].Split(',');
                foreach (var item in codes)
                {
                    if (item == draweds[i])
                    {
                        drawed += 1;
                        break;
                    }
                }
            }
            if (drawed == positionCodes.Length)
                return 1;
            return 0;
        }

        public int FixedUnset(string code, string drawedNumber, int size)
        {
            string[] codes = code.Split('@');
            List<string> draweds = drawedNumber.Split(',').ToList();
            List<string> fixedCodes = codes[0].Split(',').ToList();//胆码
            List<string> unsetCodes = codes[1].Split(',').ToList();//拖码
            int fixedCount = fixedCodes.Intersect(draweds).Count();//胆码中 个数
            int unsetCount = unsetCodes.Intersect(draweds).Count();//拖码中 个数
            int BonusCount = 0;
            if (size <= 5)
            {
                bool isFixedWinner = this.IsFixedWinner(fixedCodes, draweds);
                if (isFixedWinner == false)
                {
                    return 0;
                }
                int count = BbMath.Combin(unsetCount, size - fixedCount);
                if (count > 0)
                {
                    BonusCount = 1;
                }
            }
            else if (size == 6)
            {

                if (fixedCount == draweds.Count)
                {
                    BonusCount = BbMath.Combin(unsetCount, size - fixedCount);
                }
                else if (fixedCount == 0)
                {
                    if (fixedCodes.Count == 1 && unsetCount == 5)
                    {
                        BonusCount = 1;
                    }
                }
                else if ((size - fixedCodes.Count) >= (5 - fixedCount))
                {
                    if (fixedCount + unsetCount == 5)
                    {
                        BonusCount = 1;
                    }
                }
            }
            else
            {
                if (fixedCount == draweds.Count)
                {
                    BonusCount = 1;
                }
                else if (fixedCount == 0)
                {
                    if (fixedCodes.Count < size - 4)
                    {
                        if (unsetCount == 5)
                        {
                            BonusCount = 1;
                        }
                    }
                }
                else if ((size - fixedCodes.Count) >= (5 - fixedCount))
                {
                    if (fixedCount + unsetCount == 5)
                    {
                        BonusCount = 1;
                    }
                }
            }
            if (BonusCount > 0)
            {
                return 1;
            }
            return 0;
        }

        public int FrontAnyFixedUnset(string code, string drawedNumber, int size)
        {
            string[] codes = code.Split('@');
            string[] draweds = drawedNumber.Split(',');
            if (codes[0] != drawedNumber.Substring(0, codes[0].Length))
                return 0;
            List<string> lists = ToSingle(codes[0], codes[1], size);
            string drawed = "";
            for (int i = 0; i < size; i++)
            {
                drawed += draweds[i] + ",";
            }
            drawed = drawed.Remove(drawed.Length - 1, 1);
            string[] drawarr = drawed.Split(',');
            foreach (var item in lists)
            {
                string[] itemarr = item.Split(',');
                if (drawarr.Intersect(itemarr).Count() == size)
                {
                    return 1;
                }
            }
            return 0;
        }

        public List<string> ToSingle(string fixwdCode, string unsetCode, int size)
        {
            List<string> lists = new List<string>();
            int fixwdcount = fixwdCode.Split(',').Count();
            string[] unsetCodes = unsetCode.Split(',');
            if (size == 3)
                if (fixwdcount == 1)
                {
                    for (int i = 0; i < unsetCodes.Length; i++)
                    {
                        for (int j = i + 1; j < unsetCodes.Length; j++)
                        {
                            lists.Add(fixwdCode + ',' + unsetCodes[i] + ',' + unsetCodes[j]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < unsetCodes.Length; i++)
                    {
                        lists.Add(fixwdCode + ',' + unsetCodes[i]);
                    }
                }
            else if (size == 2)
                for (int i = 0; i < unsetCodes.Length; i++)
                {
                    lists.Add(fixwdCode + ',' + unsetCodes[i]);
                }
            return lists;
        }

        /// <summary>
        /// 判断胆码是否中奖
        /// </summary>
        /// <param name="fixedNumbers">胆码</param>
        /// <param name="drawedNumbers">开奖号码</param>
        /// <returns>胆码是否中奖</returns>
        private bool IsFixedWinner(List<string> fixedNumbers, List<string> drawedNumbers)
        {
            List<string> winnerNumbers = fixedNumbers.Intersect(drawedNumbers).ToList();
            return winnerNumbers.Count == fixedNumbers.Count;
        }
    }
}
