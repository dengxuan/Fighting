namespace Baibaocp.LotteryOrdering.Validations
{
    public sealed class Constants
    {
        /// <summary>
        /// 号码与号码直接的分隔符
        /// </summary>
        public const char SPLIT_CODE = ',';

        /// <summary>
        /// 位与位之间的分隔符
        /// </summary>
        public const char SPLIT_POSITION = '*';

        /// <summary>
        /// 胆与拖直接的分隔符
        /// </summary>
        public const char SPLIT_FIXED = '@';

        /// <summary>
        /// 花色分隔符
        /// </summary>
        public const char SPLIT_STYLOR = '|';

        /// <summary>
        /// 占位符
        /// </summary>
        public const char PLACEHOLDER = '_';

        public const char SPLIT_COMPETITIVE = '^';

        /// <summary>
        /// 单价(分)
        /// </summary>
        public const int UNIT_PRICE = 200;

        /// <summary>
        /// 追加单价(分)
        /// </summary>
        public const int ADD_UNIT_PRICE = 300;

        /// <summary>
        /// 最大投注金额(分)
        /// </summary>
        public const int MAX_AMOUNT = 2000000;

        /// <summary>
        /// 最小倍数
        /// </summary>
        public const byte MIN_TIMES = 1;

        /// <summary>
        /// 最大倍数
        /// </summary>
        public const byte MAX_TIMES = 99;

        /// <summary>
        /// 双色球红球
        /// </summary>
        public static readonly string[] SSQ_RED_CODES = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33" };

        /// <summary>
        /// 双色球蓝球
        /// </summary>
        public static readonly string[] SSQ_BLUE_CODES = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16" };

        /// <summary>
        /// 双色球红球数量
        /// </summary>
        public const int SSQ_RED_COUNT = 6;

        /// <summary>
        /// 双色球蓝球数量
        /// </summary>
        public const int SSQ_BLUE_COUNT = 1;

        /// <summary>
        /// 双色球复式红球最多数量
        /// </summary>
        public const int SSQ_RED_MAX_COUNT = 20;

        /// <summary>
        /// 双色球蓝球最多数量
        /// </summary>
        public const int SSQ_BLUE_MAX_COUNT = 16;

        /// <summary>
        /// 双色球红球胆码最小数量
        /// </summary>
        public const int SSQ_RED_FIXED_MIN_COUNT = 1;

        /// <summary>
        /// 双色球红球胆码最多数量
        /// </summary>
        public const int SSQ_RED_FIXED_MAX_COUNT = 5;

        /// <summary>
        /// 大乐透前区
        /// </summary>
        public static readonly string[] DLT_FRONT_CODES = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35" };

        /// <summary>
        /// 大乐透后区
        /// </summary>
        public static readonly string[] DLT_LATER_CODES = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

        /// <summary>
        /// 大乐透前区数量
        /// </summary>
        public const int DLT_FRONT_COUNT = 5;

        /// <summary>
        /// 大乐透后区数量
        /// </summary>
        public const int DLT_LATER_COUNT = 2;

        /// <summary>
        /// 大乐透复式前区最多数量
        /// </summary>
        public const int DLT_FRONT_MAX_COUNT = 20;

        /// <summary>
        /// 大乐透复式后区最多数量
        /// </summary>
        public const int DLT_LATER_MAX_COUNT = 12;

        /// <summary>
        /// 大乐透前区胆码最小数量
        /// </summary>
        public const int DLT_FRONT_BRAVERY_MIN_COUNT = 1;

        /// <summary>
        /// 大乐透前区胆码最大数量
        /// </summary>
        public const int DLT_LATER_BRAVERY_MAX_COUNT = 4;

        public static readonly string[] SYXW_CODES = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11" };

        /// <summary>
        /// 十一选五最大可选数
        /// </summary>
        public const int SYXW_MAX_COUNT = 11;

        /// <summary>
        /// 十一选五前一号码数量
        /// </summary>
        public const int SYXW_EXCATONE_COUNT = 1;

        /// <summary>
        /// 十一选五前二号码数量
        /// </summary>
        public const int SYXW_EXCATTWO_COUNT = 2;

        /// <summary>
        /// 十一选五前三号码数量
        /// </summary>
        public const int SYXW_EXCATTHREE_COUNT = 3;

        /// <summary>
        /// 十一选五任选二号码数量
        /// </summary>
        public const int SYXW_ANYTWO_COUNT = 2;

        /// <summary>
        /// 十一选五任选三号码数量
        /// </summary>
        public const int SYXW_ANYTHREE_COUNT = 3;

        /// <summary>
        /// 十一选五任选四号码数量
        /// </summary>
        public const int SYXW_ANYFOUR_COUNT = 4;

        /// <summary>
        /// 十一选五任选五号码数量
        /// </summary>
        public const int SYXW_ANYFIVE_COUNT = 5;

        /// <summary>
        /// 十一选五任选六号码数量
        /// </summary>
        public const int SYXW_ANYSIX_COUNT = 6;

        /// <summary>
        /// 十一选五任选七号码数量
        /// </summary>
        public const int SYXW_ANYSEVEN_COUNT = 7;

        /// <summary>
        /// 十一选五任选八号码数量
        /// </summary>
        public const int SYXW_ANYEIGHT_COUNT = 8;

        /// <summary>
        /// 快三号码
        /// </summary>
        public static readonly string[] KS_CODES = new string[] { "1", "2", "3", "4", "5", "6" };

        /// <summary>
        /// 快三和值
        /// </summary>
        public static readonly string[] KS_SUM_CODES = new string[] { "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17" };

        /// <summary>
        /// 快三三同号通选
        /// </summary>
        public const string KS_THREE_SAME_ALL_CODES = "7,7,7";

        /// <summary>
        /// 快三三连号通选
        /// </summary>
        public const string KS_THREE_SERIES_ALL_CODES = "7,8,9";

        /// <summary>
        /// 快三最大可选数
        /// </summary>
        public const int KS_MAX_COUNT = 6;

        /// <summary>
        /// 快三单式号码数量
        /// </summary>
        public const int KS_SINGLE_COUNT = 3;

        /// <summary>
        /// 快三两同号复选号码数量
        /// </summary>
        public const int KS_TOW_SAME_ALL_COUNT = 1;

        /// <summary>
        /// 快三两同号单选号码数量
        /// </summary>
        public const int KS_TOW_SAME_SINGLE_COUNT = 3;

        /// <summary>
        /// 快三两不同单选
        /// </summary>
        public const int KS_TOW_DIFF_SINGLE_COUNT = 2;

        /// <summary>
        /// 快三两不同胆码最小数量
        /// </summary>
        public const int KS_TOW_DIFF_FIXED_COUNT = 1;

        /// <summary>
        /// 快三两不同托码最小数量
        /// </summary>
        public const int KS_TOW_DIFF_UNSET_MIN_COUNT = 1;

        /// <summary>
        /// 快三两不同托码最大数量
        /// </summary>
        public const int KS_TOW_DIFF_UNSET_MAX_COUNT = 5;

        /// <summary>
        ///  快三三不同单选号码数量
        /// </summary>
        public const int KS_THREE_DIFF_SINGLE_COUNT = 3;

        /// <summary>
        /// 快三三同号单选
        /// </summary>
        public const int KS_THREE_SAME_SINGLE_COUNT = 3;

        /// <summary>
        /// 快乐十分选二
        /// </summary>
        public const int KLSF_TWO_COUNT = 2;

        /// <summary>
        /// 快乐十分选三
        /// </summary>
        public const int KLSF_THREE_COUNT = 3;

        /// <summary>
        /// 快乐十分选四
        /// </summary>
        public const int KLSF_FOUR_COUNT = 4;

        /// <summary>
        /// 快乐十分选五
        /// </summary>
        public const int KLSF_FIVE_COUNT = 5;

        #region 3D

        public static readonly string[] SD_CODES = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public static readonly string[] SD_FRONT_SUM_CODES = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27" };

        public static readonly string[] SD_ANYTHREE_SUM_CODES = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26" };

        public static readonly string[] SD_ANYSIX_SUM_CODES = new string[] { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };

        public const int SD_CODES_COUNT = 10;

        public const int SD_FRONT_CODES_MINCOUNT = 3;

        public const int SD_ANYTHREE_CODES_MINCOUNT = 2;

        public const int SD_ANYSIX_CODES_MINCOUNT = 4;

        #endregion 3D

        #region PLS
        public static readonly string[] PLS_CODES = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public static readonly string[] PLS_FRONT_SUM_CODES = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27" };

        public static readonly string[] PLS_ANYTHREE_SUM_CODES = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26" };

        public static readonly string[] PLS_ANYSIX_SUM_CODES = new string[] { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24" };

        public const int PLS_CODES_COUNT = 10;

        public const int PLS_FRONT_CODES_MINCOUNT = 3;

        /// <summary>
        /// 排列三位数
        /// </summary>
        public const int PLS_FRONT_POSITION_COUNT = 3;

        public const int PLS_ANYTHREE_CODES_MINCOUNT = 2;

        public const int PLS_ANYSIX_CODES_MINCOUNT = 4;

        #endregion PLS

        #region PLW
        public static readonly string[] PLW_CODES = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public const int PLW_CODES_COUNT = 10;

        public const int PLW_FRONT_CODES_MINCOUNT = 5;

        /// <summary>
        /// 排列五位数
        /// </summary>
        public const int PLW_FRONT_POSITION_COUNT = 5;

        #endregion PLW

        #region QXC
        public static readonly string[] QXC_CODES = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public const int QXC_CODES_COUNT = 10;

        public const int QXC_POSITION_COUNT = 7;


        #endregion QXC
    }
}
