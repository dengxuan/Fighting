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
    public class SsqCalculator : LottoLotteryCalculator
    {
        public SsqCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder) : base(iocResolver, lotteryMerchanteOrder)
        {
        }

        public override async Task<Handle> CalculateAsync()
        {
            int level = 0;
            string DrawNumber = await FindDrawNumberAsync(LotteryMerchanteOrder.LotteryId, LotteryMerchanteOrder.IssueNumber.Value);
            if (string.IsNullOrEmpty(DrawNumber))
            {
                return Handle.Waiting;
            }
            switch (LotteryMerchanteOrder.LotteryPlayId)
            {
                case (int)PlayTypes.Ssq_Single: level = SingleSsq(LotteryMerchanteOrder.InvestCode, DrawNumber); break;
                case (int)PlayTypes.Ssq_Multiple: level = CompoundSsq(LotteryMerchanteOrder.InvestCode, DrawNumber); break;
                case (int)PlayTypes.Ssq_FixedUnset: level = BraveryTowSsq(LotteryMerchanteOrder.InvestCode, DrawNumber); break;
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


        /// <summary>
        /// 双色球单式算奖
        /// </summary>
        /// <param name="code">用户选择号码</param>
        /// <param name="drawedNumber">开奖号码</param>
        /// <returns></returns>
        protected int SingleSsq(string code, string drawedNumber)
        {
            int level = 0;
            List<string> bonuslevel = new List<string>();
            string[] ArrDrawedNumber = drawedNumber.Split('*');
            List<string> ReddrawedNumber = ArrDrawedNumber[0].Split(',').ToList();
            List<string> BluedreawedNumber = ArrDrawedNumber[1].Split(',').ToList();
            string[] newcode = code.Split('*');
            List<string> RedchosedNumber = newcode[0].Split(',').ToList();
            List<string> BluechosedNumber = newcode[1].Split(',').ToList();
            level = SsqSingle(RedchosedNumber, BluechosedNumber, ReddrawedNumber, BluedreawedNumber);
            return level;
        }

        /// <summary>
        /// 双色球复式算奖
        /// </summary>
        /// <param name="code">所选号码</param>
        /// <param name="drawedNumber">开奖号码</param>
        /// <returns>奖级</returns>
        protected int CompoundSsq(string code, string drawedNumber)
        {
            int level = 0;

            string[] ArrDrawedNumber = drawedNumber.Split('*');
            List<string> ReddrawedNumber = ArrDrawedNumber[0].Split(',').ToList();
            List<string> BluedreawedNumber = ArrDrawedNumber[1].Split(',').ToList();
            string[] ComNumber = code.Split('*');
            List<string> RedNumbers = ComNumber[0].Split(',').ToList();
            List<string> BlueNumbers = ComNumber[1].Split(',').ToList();
            List<string> RedNumber = BbMath.GetPortfolio(RedNumbers, 6);
            List<string> BlueNumber = BbMath.GetPortfolio(BlueNumbers, 1);

            /* 拆分复式为单式，红蓝球用“|”分割 */
            var array = BbMath.GetArrayList(RedNumber, BlueNumber, "|");
            for (int j = 0; j < array.Count(); j++)
            {
                string[] newcode = array[j].Split('|');
                List<string> RedchosedNumber = newcode[0].Split(',').ToList();
                List<string> BluechosedNumber = newcode[1].Split(',').ToList();
                level = this.SsqSingle(RedchosedNumber, BluechosedNumber, ReddrawedNumber, BluedreawedNumber);
                if (level > 0)
                {
                    break;
                }
            }
            return level;
        }

        /// <summary>
        /// 双色球胆拖算奖
        /// </summary>
        /// <param name="code">所选号码</param>
        /// <param name="drawedNumber">开奖号码</param>
        /// <returns></returns>
        protected int BraveryTowSsq(string code, string drawedNumber)
        {
            int level = 0;
            string[] ArrDrawedNumber = drawedNumber.Split('*');
            List<string> ReddrawedNumber = ArrDrawedNumber[0].Split(',').ToList();
            List<string> BluedreawedNumber = ArrDrawedNumber[1].Split(',').ToList();
            List<string> array = new List<string>();
            string[] TowNumber = code.Split('*');
            string[] RedBraveryTow = TowNumber[0].Split('@');
            string[] RedBraveryNumbers = RedBraveryTow[0].Split(',');//前区胆码
            List<string> RedTowBumbers = RedBraveryTow[1].Split(',').ToList(); //前区拖码

            List<string> RedBraveryList = new List<string> { RedBraveryTow[0] };//将前区胆码转为泛型

            List<string> RedTowNumber = BbMath.GetPortfolio(RedTowBumbers, 6 - RedBraveryNumbers.Count());//将拖码组合
            List<string> RedNumberCombination = BbMath.GetArrayList(RedBraveryList, RedTowNumber, ",");//得到胆码拖码所有组合
            string[] BlueBraveryTow = TowNumber[1].Split(',');

            array = BbMath.GetArrayList(RedNumberCombination, BlueBraveryTow.ToList(), "|");

            for (Int32 j = 0; j < array.Count(); j++)
            {
                string[] newcode = array[j].Split('|');
                List<string> FronchosedNumber = newcode[0].Split(',').ToList();
                List<string> BackchosedNumber = newcode[1].Split(',').ToList();
                level = this.SsqSingle(FronchosedNumber, BackchosedNumber, ReddrawedNumber, BluedreawedNumber);
                if (level > 0)
                {
                    break;
                }
            }
            return level;
        }


        /// <summary>
        /// 双色球单式算奖
        /// </summary>
        /// <param name="FronchosedNumber">红球选择号码</param>
        /// <param name="BackchosedNumber">篮球选择号码</param>
        /// <param name="BackdreawedNumber">红球开奖号码</param>
        /// <param name="FrondrawedNumber">篮球开奖号码</param>
        /// <param name="investType"></param>
        /// <returns>是否中奖</returns>
        protected int SsqSingle(List<string> RedchosedNumber, List<string> BluechosedNumber, List<string> ReddrawedNumber, List<string> BluedreawedNumber)
        {
            int level = 0;
            Int32 redNum = ReddrawedNumber.Intersect(RedchosedNumber).Count();
            Int32 blueNum = BluedreawedNumber.Intersect(BluechosedNumber).Count();
            if (redNum == 6 && blueNum == 1)
            {
                level = 1;
            }
            else if (redNum == 6)
            {
                level = 2;
            }
            else if (redNum == 5 && blueNum == 1)
            {
                level = 3;
            }
            else if ((redNum == 5 && blueNum == 0) || (redNum == 4 && blueNum == 1))
            {
                level = 4;
            }
            else if ((redNum == 4 && blueNum == 0) || (redNum == 3 && blueNum == 1))
            {
                level = 5;
            }
            else if ((redNum == 2 && blueNum == 1) || (redNum == 1 && blueNum == 1) || (redNum == 0 && blueNum == 1))
            {
                level = 6;
            }
            return level;
        }
    }
}
