using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryCalculating.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.Math;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Calculators
{
    public class QxcCalculator : LottoLotteryCalculator
    {
        public QxcCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder) : base(iocResolver, lotteryMerchanteOrder)
        {
        }

        public override async Task<Handle> CalculateAsync()
        {
            int level = 0;
            string drawNumber = await FindDrawNumberAsync(LotteryMerchanteOrder.LotteryId, (int)LotteryMerchanteOrder.IssueNumber);
            if (string.IsNullOrEmpty(drawNumber))
            {
                return Handle.Waiting;
            }
            level = CalculateQxc(LotteryMerchanteOrder.InvestCode, drawNumber);
            if (level > 0)
            {
                return Handle.Winner;
            }
            else
            {
                return Handle.Losing;
            }
        }

        /// <summary>
        /// 大乐透单式算奖
        /// </summary>
        /// <param name="code">用户选择号码</param>
        /// <param name="drawedNumber">开奖号码</param>
        /// <returns></returns>
        protected int CalculateQxc(string code, string drawedNumber)
        {
            int level = 0;
            string[] codelist = code.Split('*');
            string[] drawlist = drawedNumber.Split(',');
            int n = 0;
            for (int i = 0; i < drawlist.Length; i++)
            {
                if (codelist[i].Split(',').Contains(drawlist[i]))
                {
                    n += 1;
                    if (n > level)
                    {
                        level = n;
                    }
                }
                else
                {
                    n = 0;
                }
            }
            if (level > 1)
            {
                level = 1;
            }
            else
            {
                level = 0;
            }
            return level;
        }
    }
}
