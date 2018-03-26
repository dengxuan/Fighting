using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fighting.Math
{
    public class BbMath
    {
        /// <summary>
        /// 计算C(N,M)
        /// </summary>
        /// <param name="n">N</param>
        /// <param name="m">M</param>
        /// <returns>组合数</returns>
        public static Int32 Combin(Int32 n, Int32 m)
        {
            /* 不处理负数 */
            if (n < 0 || m < 0)
            {
                return 0;
            }

            Int32 t = n - m;
            Int64 factN = 1;
            Int64 factM = 1;
            if (m > n)
            {
                return 0;
            }
            if (m == n)
            {
                return 1;
            }
            if (t < m)
            {
                m = t;
            }
            for (int i = 1; i <= m; n--, m--)
            {
                factN = factN * n;
                factM = factM * m;
            }
            return (Int32)(factN / factM);
        }

        /// <summary>
        /// 计算组合
        /// </summary>
        /// <param name="numbers">待组合数据</param>
        /// <param name="n">以几个为一组</param>
        /// <returns>组合后数据</returns>
        public static List<string> GetPortfolio(List<string> numbers, int n)
        {

            int m = numbers.Count;  //参数数组长度
            int l = 1;               //构造数组长度标志
            for (int s = m; s > m - n; s--) //获得排列数组长度L
            {
                l = l * s;
            }

            for (int z = n; z > 1; z--)
            {
                l = l / z;
            }

            string[] rs = new string[l];           //返回结果

            string str = "";                     //将第一种排列存入数组RS（即所有1都在左边的情况）
            for (int x = 0; x < n; x++)
            {
                str = str + numbers[x] + ",";
            }
            rs[0] = str;

            int[] a = new int[m];    //构造下标数组
            Boolean flag = true;     //循环开关
            int k = 1;               //返回结果数组长度（自增长）

            for (int i = 0; i < m; i++)   //初始化构造下标数组
            {
                if (i < n)
                    a[i] = 1;
                else
                    a[i] = 0;
            }
            do
            {
                flag = false;                        //初始FLAG
                int zerocount = 0;                  //10转换01前的0的个数
                for (int i = 0; i < m - 1; i++)
                {
                    if (a[i] == 0)                   //记录前0个数（非0即1），可以通过这个参数进行1的前移 
                    {
                        zerocount++;
                    }
                    if (a[i] == 1 && a[i + 1] == 0)        //10变成01 
                    {
                        a[i] = 0; a[i + 1] = 1;

                        flag = true;                 //如果成功转换，flag设置为0，如果没有证明所以1已经移动到最后，故可以跳出DO循环

                        for (int j = 0; j < i; j++)       //10转换01前的所有1前移操作
                        {
                            if (j < i - zerocount)
                                a[j] = 1;
                            else
                                a[j] = 0;
                        }
                        string returnstr = "";       //用于存储变化后的构造数组

                        for (int kk = 0; kk < m; kk++)    //通过构造数组下标，得到需要的返回串
                        {
                            if (a[kk] == 1)
                            {
                                returnstr = returnstr + numbers[kk] + ",";
                            }
                        }

                        rs[k] = returnstr;           //将串存入RS数组，用于返回   
                        i = m;                       //转换了第一个10后，就要跳出该次FOR循环，所以将i直接置成m
                        k++;                       //rs返回数组下标向后推一位，用于存储下个返回串
                    }
                }
            } while (flag == true);

            return rs.ToList();
        }

        /// <summary>
        /// 计算组合
        /// </summary>
        /// <param name="BraveryNumber">胆码集合(02,03)</param>
        /// <param name="TowNumber">拖码集合</param>
        /// <param name="Delimiter">分隔符</param>
        /// <returns>组合后集合</returns>
        public static List<string> GetArrayList(List<string> BraveryNumber, List<string> TowNumber, string Delimiter)
        {
            List<List<string>> list = new List<List<string>>() { BraveryNumber, TowNumber };
            var array = list.Aggregate((m, n) => m.SelectMany(t1 => n.Select(t2 => t1 + Delimiter + t2).ToList()).ToList()).ToList();
            return array.ToList();
        }
    }
}
