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
    public class SdPlsCalculator : NumericLotteryCalculator
    {
        public SdPlsCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder) : base(iocResolver, lotteryMerchanteOrder)
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
                case (int)PlayTypes.Pls_AntThreeFixedUnset:
                    level = this.AnyThreeFixed(LotteryMerchanteOrder.InvestCode, drawNumber);
                    break;
                case (int)PlayTypes.Pls_AnySixFixedUnset:
                    level = this.AnySixFixed(LotteryMerchanteOrder.InvestCode, drawNumber);
                    break;
                case (int)PlayTypes.Pls_AnySixMultiple:
                case (int)PlayTypes.Pls_AnySixSingle:
                case (int)PlayTypes.Sd_AnySixMultiple:
                case (int)PlayTypes.Sd_AnySixSingle:
                    level = this.AnySixSdPls(LotteryMerchanteOrder.InvestCode, drawNumber);
                    break;
                case (int)PlayTypes.Pls_AnySixSum:
                case (int)PlayTypes.Pls_AnyThreeSum:
                case (int)PlayTypes.Pls_FrontSum:
                    level = this.SumSdPLs(LotteryMerchanteOrder.InvestCode, drawNumber);
                    break;
                case (int)PlayTypes.Pls_AnyThreeMultiple:
                case (int)PlayTypes.Pls_AnyThreeSingle:
                case (int)PlayTypes.Sd_AnyThreeMultiple:
                case (int)PlayTypes.Sd_AnyThreeSingle:
                    level = this.AnyThreeSdPls(LotteryMerchanteOrder.InvestCode, drawNumber);
                    break;
                case (int)PlayTypes.Pls_FrontCombin:
                    level = this.FrontCombin(LotteryMerchanteOrder.InvestCode, drawNumber);
                    break;
                case (int)PlayTypes.Pls_FrontCombinFixedUnset:
                    level = this.FrontCombinFixed(LotteryMerchanteOrder.InvestCode, drawNumber);
                    break;
                case (int)PlayTypes.Pls_FrontMultiple:
                case (int)PlayTypes.Pls_FrontSingle:
                case (int)PlayTypes.Sd_FrontMultiple:
                case (int)PlayTypes.Sd_FrontSingle:
                case (int)PlayTypes.Plw_FrontMultiple:
                case (int)PlayTypes.Plw_FrontSingle:
                    level = this.FrontSdPls(LotteryMerchanteOrder.InvestCode, drawNumber);
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

        protected int FrontSdPls(string code, string drawnumber)
        {
            int level = 1;
            string[] codes = code.Split('*');
            string[] draws = drawnumber.Split(',');
            for (int i = 0; i < codes.Length; i++)
            {
                string[] nums = codes[i].Split(',');
                if (!nums.Contains(draws[i]))
                {
                    return 0;
                }
            }
            return level;
        }

        protected int SumSdPLs(string code, string drawnumber)
        {
            int level = 0;
            string[] codes = code.Split(',');
            string drawsum = drawnumber.Split(',').ToList().Sum(selector => Convert.ToInt32(selector)).ToString();
            for (int i = 0; i < code.Length; i++)
            {
                if (codes.Contains(drawsum))
                {
                    return 1;
                }
            }
            return level;
        }

        protected int AnyThreeSdPls(string code, string drawnumber)
        {
            int level = 1;
            string[] codes = code.Split(',');
            string[] draws = drawnumber.Split(',');
            if (draws.Distinct().Count() != 2)
            {
                return 0;
            }
            string num = string.Empty;
            string num1 = string.Empty;
            foreach (var v in codes.GroupBy(x => x).Select(x => new { k = x.Key, c = x.Count() }))
            {
                if (v.c == 2)
                {
                    num = v.k;
                }
            }
            foreach (var v1 in draws.GroupBy(x1 => x1).Select(x1 => new { k1 = x1.Key, c1 = x1.Count() }))
            {
                if (v1.c1 == 2)
                {
                    num1 = v1.k1;
                }
            }
            if (num != num1)
            {
                return 0;
            }
            if (codes.Distinct().Intersect(draws.Distinct()).Count() < 2)
            {
                return 0;
            }
            return level;
        }

        protected int AnySixSdPls(string code, string drawnumber)
        {
            int level = 1;
            string[] codes = code.Split(',');
            string[] draws = drawnumber.Split(',');
            if (draws.Distinct().Count() != 3)
            {
                return 0;
            }
            if (codes.Distinct().Intersect(draws.Distinct()).Count() < 3)
            {
                return 0;
            }
            return level;
        }

        protected int FrontCombin(string code, string drawnumber)
        {
            int level = 1;
            string[] codes = code.Split(',');
            string[] draws = drawnumber.Split(',');
            if (codes.Intersect(draws).Count() < 3)
            {
                return 0;
            }
            return level;
        }

        protected int FrontCombinFixed(string code, string drawnumber)
        {
            int level = 1;
            string[] codes = code.Split('@');
            string[] dancode = codes[0].Split(',');
            string[] tuocode = codes[1].Split(',');
            string[] draws = drawnumber.Split(',');
            if (dancode.Intersect(draws).Count() != dancode.Count())
            {
                return 0;
            }
            if (tuocode.Intersect(draws).Count() + dancode.Count() < 3)
            {
                return 0;
            }
            return level;
        }

        protected int AnyThreeFixed(string code, string drawnumber)
        {
            int level = 1;
            string[] codes = code.Split('@');
            string[] draws = drawnumber.Split(',');
            if (draws.Distinct().Count() != 2)
            {
                return 0;
            }
            if (draws.Contains(codes[0]))
            {
                string[] tuocode = codes[1].Split(',');
                if (tuocode.Intersect(draws).Count() + 1 < 2)
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            return level;
        }

        protected int AnySixFixed(string code, string drawnumber)
        {
            int level = 1;
            string[] codes = code.Split('@');
            string[] dancode = codes[1].Split(',');
            string[] draws = drawnumber.Split(',');
            if (draws.Distinct().Count() != 3)
            {
                return 0;
            }
            if (draws.Intersect(dancode).Count() == dancode.Count())
            {
                string[] tuocode = codes[1].Split(',');
                if (tuocode.Intersect(draws).Count() + dancode.Count() < 3)
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
            return level;
        }
    }
}
