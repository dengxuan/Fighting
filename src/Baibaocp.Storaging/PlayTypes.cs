namespace Baibaocp.Storaging.Entities
{
    public enum PlayTypes
    {
        /// <summary>
        /// 双色球单式
        /// </summary>
        Ssq_Single = 10011071,

        /// <summary>
        /// 双色球复式
        /// </summary>
        Ssq_Multiple = 10012071,

        /// <summary>
        /// 双色球胆拖
        /// </summary>
        Ssq_FixedUnset = 10013071,

        /// <summary>
        /// 大乐透单式
        /// </summary>
        Dlt_Single = 10021071,

        /// <summary>
        /// 大乐透复式
        /// </summary>
        Dlt_Multiple = 10022071,

        /// <summary>
        /// 大乐透胆拖
        /// </summary>
        Dlt_FixedUnset = 10023071,

        /// <summary>
        /// 七星彩单式
        /// </summary>
        Qxc_Single = 10051071,

        /// <summary>
        /// 七星彩复式
        /// </summary>
        Qxc_Multiple = 10052071,

        /// <summary>
        /// 快三两同号单选
        /// </summary>
        Ks_TowSameSingle = 10071034,

        /// <summary>
        /// 快三两同号复选
        /// </summary>
        Ks_TowSameAll = 10074014,

        /// <summary>
        /// 两不同单式
        /// </summary>
        Ks_TowDiffSingle = 10071025,

        /// <summary>
        /// 三同号单选
        /// </summary>
        Ks_ThreeSameSingle = 10071036,

        /// <summary>
        /// 三同号通选
        /// </summary>
        Ks_ThreeSameAll = 10074036,

        /// <summary>
        /// 三不同单选
        /// </summary>
        Ks_ThreeDiffSingle = 10071035,

        /// <summary>
        /// 三连号通选
        /// </summary>
        Ks_ThreeSeriesAll = 10074037,

        /// <summary>
        /// 和值
        /// </summary>
        Ks_SumValue = 10071018,

        /// <summary>
        /// 前一单式
        /// </summary>
        Syxw_FrontOneSingle = 10061011,

        /// <summary>
        /// 前一复式
        /// </summary>
        Syxw_FrontOneMultiple = 10062011,

        /// <summary>
        /// 前二直选单式
        /// </summary>
        Syxw_FrontTowFixedPositionSingle= 10061021,

        /// <summary>
        /// 前二直选复式
        /// </summary>
        Syxw_FrontTowFixedPositionMultiple = 10062021,

        /// <summary>
        /// 前二组选单式
        /// </summary>
        Syxw_FrontTowAnyPositionSingle= 10061022,

        /// <summary>
        /// 前二组选复式
        /// </summary>
        Syxw_FrontTowAnyPositionMultiple= 10062022,

        /// <summary>
        /// 前二组选胆拖
        /// </summary>
        Syxw_FrontTowAnyPositionFixedUnset= 10063022,

        /// <summary>
        /// 前三直选单式
        /// </summary>
        Syxw_FrontThreeFixedPositionSingle= 10061031,

        /// <summary>
        /// 前三直选复式
        /// </summary>
        Syxw_FrontThreeFixedPositionMultiple = 10062031,

        /// <summary>
        /// 前三组选单式
        /// </summary>
        Syxw_FrontThreeAnyPositionSingle= 10061032,

        /// <summary>
        /// 前三组选复式
        /// </summary>
        Syxw_FrontThreeAnyPositionMultiple= 10062032,

        /// <summary>
        /// 前三组选胆拖
        /// </summary>
        Syxw_FrontThreeAnyPositionFixedUnset= 10063032,

        /// <summary>
        /// 任二单式
        /// </summary>
        Syxw_AnyTowSingle = 10061023,

        /// <summary>
        /// 任二复式
        /// </summary>
        Syxw_AnyTownMultiple = 10062023,

        /// <summary>
        /// 任二胆拖
        /// </summary>
        Syxw_AnyTowFixedUnset = 10063023,

        /// <summary>
        /// 任三单式
        /// </summary>
        Syxw_AnyThreeSingle = 10061033,

        /// <summary>
        /// 任三复式
        /// </summary>
        Syxw_AnyThreeMultiple = 10062033,

        /// <summary>
        /// 任三胆拖
        /// </summary>
        Syxw_AnyThreeFixedUnset = 10063033,

        /// <summary>
        /// 任四单式
        /// </summary>
        Syxw_AnyFourSingle = 10061043,

        /// <summary>
        /// 任四复式
        /// </summary>
        Syxw_AnyFourMultiple = 10062043,

        /// <summary>
        /// 任四胆拖
        /// </summary>
        Syxw_AnyFourFixedUnset = 10063043,

        /// <summary>
        /// 任五单式
        /// </summary>
        Syxw_AnyFiveSingle = 10061053,

        /// <summary>
        /// 任五复式
        /// </summary>
        Syxw_AnyFiveMultiple = 10062053,

        /// <summary>
        /// 任五胆拖
        /// </summary>
        Syxw_AnyFiveFixedUnset = 10063053,

        /// <summary>
        /// 任六单式
        /// </summary>
        Syxw_AnySixSingle = 10061063,

        /// <summary>
        /// 任六复式
        /// </summary>
        Syxw_AnySixMultiple = 10062063,

        /// <summary>
        /// 任六胆拖
        /// </summary>
        Syxw_AnySixFixedUnset = 10063063,

        /// <summary>
        /// 任七单式
        /// </summary>
        Syxw_AnySevenSingle = 10061073,

        /// <summary>
        /// 任七复式
        /// </summary>
        Syxw_AnySevenMultiple = 10062073,

        /// <summary>
        /// 任七胆拖
        /// </summary>
        Syxw_AnySevenFixedUnset = 10063073,

        /// <summary>
        /// 任八单式
        /// </summary>
        Syxw_AnyEightSingle = 10061083,

        /// <summary>
        /// 任八复式
        /// </summary>
        Syxw_AnyEightMultiple = 10062083,

        /// <summary>
        /// 任八胆拖
        /// </summary>
        Syxw_AnyEightFixedUnset = 10063083,

        /// <summary>
        /// 单关
        /// </summary>
        Jc_1X1 = 500,

        /// <summary>
        /// 2串1
        /// </summary>
        Jc_2X1 = 502,

        /// <summary>
        /// 3串1
        /// </summary>
        Jc_3X1 = 503,

        /// <summary>
        /// 4串1
        /// </summary>
        Jc_4X1 = 504,

        /// <summary>
        /// 5串1
        /// </summary>
        Jc_5X1 = 505,

        /// <summary>
        /// 6串1
        /// </summary>
        Jc_6X1 = 506,

        /// <summary>
        ///7串1
        /// </summary>
        Jc_7X1 = 507,

        /// <summary>
        /// 8串1
        /// </summary>
        Jc_8X1 = 508,

        /// <summary>
        ///3串3
        /// </summary>
        Jc_3X3 = 526,

        /// <summary>
        /// 3串4
        /// </summary>
        Jc_3X4 = 527,

        /// <summary>
        /// 4串4
        /// </summary>
        Jc_4X4 = 539,

        /// <summary>
        /// 4串5
        /// </summary>
        Jc_4X5 = 540,

        /// <summary>
        /// 4串6
        /// </summary>
        Jc_4X6 = 528,

        /// <summary>
        /// 4串11
        /// </summary>
        Jc_4X11 = 529,

        /// <summary>
        /// 5串5
        /// </summary>
        Jc_5X5 = 544,

        /// <summary>
        /// 5串6
        /// </summary>
        Jc_5X6 = 545,

        /// <summary>
        /// 6串10
        /// </summary>
        Jc_5X10 = 530,

        /// <summary>
        /// 5串16
        /// </summary>
        Jc_5X16 = 541,

        /// <summary>
        /// 5串20
        /// </summary>
        Jc_5X20 = 531,

        /// <summary>
        /// 5串26
        /// </summary>
        Jc_5X26 = 532,

        /// <summary>
        ///6串6
        /// </summary>
        Jc_6X6 = 549,

        /// <summary>
        /// 6串7
        /// </summary>
        Jc_6X7 = 550,

        /// <summary>
        /// 6串15
        /// </summary>
        Jc_6X15 = 533,

        /// <summary>
        /// 6串20
        /// </summary>
        Jc_6X20 = 542,

        /// <summary>
        /// 6串22
        /// </summary>
        Jc_6X22 = 546,

        /// <summary>
        /// 6串35
        /// </summary>
        Jc_6X35 = 534,

        /// <summary>
        ///6串42
        /// </summary>
        Jc_6X42 = 543,

        /// <summary>
        ///6串50
        /// </summary>
        Jc_6X50 = 535,

        /// <summary>
        /// 6串57
        /// </summary>
        Jc_6X57 = 536,

        /// <summary>
        /// 7串7
        /// </summary>
        Jc_7X7 = 553,

        /// <summary>
        /// 7串8
        /// </summary>
        Jc_7X8 = 554,

        /// <summary>
        /// 7串21
        /// </summary>
        Jc_7X21 = 551,

        /// <summary>
        /// 7串35
        /// </summary>
        Jc_7X35 = 547,

        /// <summary>
        /// 7串120
        /// </summary>
        Jc_7X120 = 537,

        /// <summary>
        /// 8串8
        /// </summary>
        Jc_8X8 = 556,

        /// <summary>
        /// 8串9
        /// </summary>
        Jc_8X9 = 557,

        /// <summary>
        /// 8串28
        /// </summary>
        Jc_8X28 = 555,

        /// <summary>
        /// 8串56
        /// </summary>
        Jc_8X56 = 552,

        /// <summary>
        /// 8串70
        /// </summary>
        Jc_8X70 = 548,

        /// <summary>
        /// 8串247
        /// </summary>
        Jc_8X247 = 538,

        /// <summary>
        /// 直选单式
        /// </summary>
        Sd_FrontSingle = 10031031,

        /// <summary>
        /// 直选复式
        /// </summary>
        Sd_FrontMultiple = 10032031,

        /// <summary>
        /// 直选和值
        /// </summary>
        Sd_FrontSum = 10033031,

        /// <summary>
        /// 组三单式
        /// </summary>
        Sd_AnyThreeSingle = 10031032,

        /// <summary>
        /// 组三复式
        /// </summary>
        Sd_AnyThreeMultiple = 10032032,

        /// <summary>
        /// 组三和值
        /// </summary>
        Sd_AnyThreeSum = 10033032,

        /// <summary>
        /// 组六单式
        /// </summary>
        Sd_AnySixSingle = 10031062,

        /// <summary>
        /// 组六复式
        /// </summary>
        Sd_AnySixMultiple = 10032062,

        /// <summary>
        /// 组六和值
        /// </summary>
        Sd_AnySixSum = 10033062,

        /// <summary>
        /// 直选单式
        /// </summary>
        Pls_FrontSingle = 10041031,

        /// <summary>
        /// 直选复式
        /// </summary>
        Pls_FrontMultiple = 10042031,

        /// <summary>
        /// 直选和值
        /// </summary>
        Pls_FrontSum = 10043031,

        /// <summary>
        /// 直选组合
        /// </summary>
        Pls_FrontCombin = 10044031,

        /// <summary>
        /// 直选组合胆拖
        /// </summary>
        Pls_FrontCombinFixedUnset = 10045031,

        /// <summary>
        /// 组三单式
        /// </summary>
        Pls_AnyThreeSingle = 10041032,

        /// <summary>
        /// 组三复式
        /// </summary>
        Pls_AnyThreeMultiple = 10042032,

        /// <summary>
        /// 组三和值
        /// </summary>
        Pls_AnyThreeSum = 10043032,

        /// <summary>
        /// 组三胆拖
        /// </summary>
        Pls_AntThreeFixedUnset = 10044032,

        /// <summary>
        /// 组六单式
        /// </summary>
        Pls_AnySixSingle = 10041062,

        /// <summary>
        /// 组六复式
        /// </summary>
        Pls_AnySixMultiple = 10042062,

        /// <summary>
        /// 组六和值
        /// </summary>
        Pls_AnySixSum = 10043062,

        /// <summary>
        /// 组六胆拖
        /// </summary>
        Pls_AnySixFixedUnset = 10044062,


        /// <summary>
        /// 直选单式
        /// </summary>
        Plw_FrontSingle = 10051051,

        /// <summary>
        /// 直选复式
        /// </summary>
        Plw_FrontMultiple = 10052051,

        /// <summary>
        /// 14场单式
        /// </summary>
        Sfc_Single = 20101001,

        /// <summary>
        /// 14场复式
        /// </summary>
        Sfc_Multiple = 20101002,

        /// <summary>
        /// 任九单式
        /// </summary>
        Rj_Single = 20102001,

        /// <summary>
        /// 任九复式
        /// </summary>
        Rj_Multiple = 20102002,

        /// <summary>
        /// 任九胆拖
        /// </summary>
        Rj_FixedUnset = 20102003,
    }
}