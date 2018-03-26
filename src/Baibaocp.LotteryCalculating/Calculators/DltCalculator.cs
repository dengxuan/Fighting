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
    public class DltCalculator : LotteryCalculator
    {
        private readonly ILotteryPhaseApplicationService _LotteryPhaseApplicationService;
        public DltCalculator(ILotteryPhaseApplicationService LotteryPhaseApplicationService, LotteryMerchanteOrder lotteryMerchanteOrder) : base(lotteryMerchanteOrder)
        {
            _LotteryPhaseApplicationService = LotteryPhaseApplicationService;
        }
        public override async Task<Handle> CalculateAsync()
        {
            int level = 0;
            LotteryPhase lotteryPhase = await _LotteryPhaseApplicationService.FindLotteryPhase(LotteryMerchanteOrder.LotteryId, (int)LotteryMerchanteOrder.IssueNumber);
            string DrawNumber = lotteryPhase.DrawNumber;
            if (DrawNumber != "")
            {
                if (LotteryMerchanteOrder.LotteryPlayId == (int)PlayTypes.Dlt_Single)
                {
                    level = SingleDlt(LotteryMerchanteOrder.InvestCode, DrawNumber);
                }
                if (LotteryMerchanteOrder.LotteryPlayId == (int)PlayTypes.Dlt_Multiple)
                {
                    level = CompoundDlt(LotteryMerchanteOrder.InvestCode, DrawNumber);
                }
                if (LotteryMerchanteOrder.LotteryPlayId == (int)PlayTypes.Dlt_FixedUnset)
                {
                    level = BraveryTowDlt(LotteryMerchanteOrder.InvestCode, DrawNumber);
                }
                if (level > 0)
                {
                    return Handle.Waiting;
                }
                else {
                    return Handle.Losing;
                }
            }
            else {
                return Handle.Waiting;
            }
        }

        /// <summary>
        /// 大乐透单式算奖
        /// </summary>
        /// <param name="code">用户选择号码</param>
        /// <param name="drawedNumber">开奖号码</param>
        /// <returns></returns>
        protected int SingleDlt(string code, string drawedNumber)
        {
            int level = 0;
            List<string> bonuslevel = new List<string>();
            string[] ArrDrawedNumber = drawedNumber.Split('*');
            List<string> FrondrawedNumber = ArrDrawedNumber[0].Split(',').ToList();
            List<string> BackdreawedNumber = ArrDrawedNumber[1].Split(',').ToList();
            string[] newcode = code.Split('*');
            List<string> FronchosedNumber = newcode[0].Split(',').ToList();
            List<string> BackchosedNumber = newcode[1].Split(',').ToList();
            level = DltSingle(FronchosedNumber, BackchosedNumber, FrondrawedNumber, BackdreawedNumber);
            return level;
        }

        /// <summary>
        /// 大乐透复式算奖
        /// </summary>
        /// <param name="code">所选号码</param>
        /// <param name="drawedNumber">开奖号码</param>
        /// <param name="investType">是否追加</param>
        /// <param name="level">奖级</param>
        /// <returns>中奖金额</returns>
        protected int CompoundDlt(string code, string drawedNumber)
        {
            int level = 0;

            string[] ArrDrawedNumber = drawedNumber.Split('*');
            List<string> FrondrawedNumber = ArrDrawedNumber[0].Split(',').ToList();
            List<string> BackdreawedNumber = ArrDrawedNumber[1].Split(',').ToList();
            string[] ComNumber = code.Split('*');
            List<string> FrontNumbers = ComNumber[0].Split(',').ToList();
            List<string> BackNumbers = ComNumber[1].Split(',').ToList();
            List<string> FrontNumber = BbMath.GetPortfolio(FrontNumbers, 5);
            List<string> BackNumber = BbMath.GetPortfolio(BackNumbers, 2);

            /* 拆分复式为单式，前后区用“|”分割 */
            var array = BbMath.GetArrayList(FrontNumber, BackNumber, "|");
            for (int j = 0; j < array.Count(); j++)
            {
                string[] newcode = array[j].Split('|');
                List<string> FronchosedNumber = newcode[0].Split(',').ToList();
                List<string> BackchosedNumber = newcode[1].Split(',').ToList();
                level = this.DltSingle(FronchosedNumber, BackchosedNumber, FrondrawedNumber, BackdreawedNumber);
                if (level > 0)
                {
                    break;
                }
            }
            return level;
        }


        /// <summary>
        /// 大乐透胆拖算奖
        /// </summary>
        /// <param name="code">所选号码</param>
        /// <param name="drawedNumber">开奖号码</param>
        /// <returns></returns>
        protected int BraveryTowDlt(string code, string drawedNumber)
        {
            int level = 0;
            string[] ArrDrawedNumber = drawedNumber.Split('*');
            List<string> FrondrawedNumber = ArrDrawedNumber[0].Split(',').ToList();
            List<string> BackdreawedNumber = ArrDrawedNumber[1].Split(',').ToList();
            List<string> array = new List<string>();
            string[] TowNumber = code.Split('*');
            string[] FrontBraveryTow = TowNumber[0].Split('@');
            List<string> FrontNumberCombination = new List<string>();
            if (FrontBraveryTow.Length == 1)
            {
                List<string> FrontBraveryList = FrontBraveryTow[0].Split(',').ToList();//将前区胆码转为list
                FrontNumberCombination = BbMath.GetPortfolio(FrontBraveryList, 5);//得到胆码拖码所有组合
            }
            else if (FrontBraveryTow.Length == 2)
            {
                string[] FrontBraveryNumbers = FrontBraveryTow[0].Split(',');//前区胆码
                List<string> FrontTowBumbers = FrontBraveryTow[1].Split(',').ToList(); //前区拖码

                List<string> FrontBraveryList = new List<string> { FrontBraveryTow[0] };//将前区胆码转为泛型

                List<string> FrontTowNumber = BbMath.GetPortfolio(FrontTowBumbers, 5 - FrontBraveryNumbers.Count());//将拖码组合
                FrontNumberCombination = BbMath.GetArrayList(FrontBraveryList, FrontTowNumber, ",");//得到胆码拖码所有组合
            }
            string[] BackBraveryTow = TowNumber[1].Split('@');

            if (BackBraveryTow.Length == 1)
            {
                //后区无胆码
                List<string> BackBraveryTows = BackBraveryTow[0].Split(',').ToList();
                List<string> BackTowNumbers = BbMath.GetPortfolio(BackBraveryTows, 2);
                array = BbMath.GetArrayList(FrontNumberCombination, BackTowNumbers, "|");
            }
            else
            {
                //后区有胆码
                List<string> BackBraveryNumber = BackBraveryTow[0].Split(',').ToList();
                List<string> BackBraveryList = new List<string> { BackBraveryTow[0] };
                List<string> BackTowNumber = BbMath.GetPortfolio(BackBraveryTow[1].Split(',').ToList(), 1);
                List<string> BackNumberCombination = BbMath.GetArrayList(BackBraveryList, BackTowNumber, ",");
                array = BbMath.GetArrayList(FrontNumberCombination, BackNumberCombination, "|");
            }
            for (Int32 j = 0; j < array.Count(); j++)
            {
                string[] newcode = array[j].Split('|');
                List<string> FronchosedNumber = newcode[0].Split(',').ToList();
                List<string> BackchosedNumber = newcode[1].Split(',').ToList();
                level = this.DltSingle(FronchosedNumber, BackchosedNumber, FrondrawedNumber, BackdreawedNumber);
                if (level > 0)
                {
                    break;
                }
            }
            return level;
        }

        /// <summary>
        /// 大乐透单式开奖
        /// </summary>
        /// <param name="FronchosedNumber">前区选择号码</param>
        /// <param name="BackchosedNumber">后区选择号码</param>
        /// <param name="BackdreawedNumber">前区开奖号码</param>
        /// <param name="FrondrawedNumber">后区开奖号码</param>
        /// <param name="investType"></param>
        /// <returns>奖金</returns>
        protected int DltSingle(List<string> FronchosedNumber, List<string> BackchosedNumber, List<string> FrondrawedNumber, List<string> BackdreawedNumber)
        {
            int level = 0;
            Int32 firsNum = FrondrawedNumber.Intersect(FronchosedNumber).Count();
            Int32 secNum = BackdreawedNumber.Intersect(BackchosedNumber).Count();
            if (firsNum == 5 && secNum == 2)
            {
                level = 1;
            }
            else if (firsNum == 5 && secNum == 1)
            {
                level = 2;
            }
            else if ((firsNum == 5 && secNum == 0) || (firsNum == 4 && secNum == 2))
            {
                level = 3;
            }
            else if ((firsNum == 4 && secNum == 1) || (firsNum == 3 && secNum == 2))
            {
                level = 4;
            }
            else if ((firsNum == 4 && secNum == 0) || (firsNum == 3 && secNum == 1) || (firsNum == 2 && secNum == 2))
            {
                level = 5;
            }
            else if ((firsNum == 3 && secNum == 0) || (firsNum == 2 && secNum == 1) || (firsNum == 1 && secNum == 2) || (firsNum == 0 && secNum == 2))
            {
                level = 6;
            }
            return level;
        }
    }
}
