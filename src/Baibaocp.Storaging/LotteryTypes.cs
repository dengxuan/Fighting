namespace Baibaocp.Storaging.Entities
{
    /// <summary>
    /// 采种编号
    /// </summary>
    public enum LotteryTypes
    {
        /// <summary>
        /// 双色球
        /// </summary>
        Ssq = 1,

        /// <summary>
        /// 大乐透
        /// </summary>
        Dlt = 2,

        /// <summary>
        /// 3D
        /// </summary>
        Sd = 30,

        /// <summary>
        /// 排列三
        /// </summary>
        Pls = 31,

        /// <summary>
        /// 排列五
        /// </summary>
        Plw = 40,

        /// <summary>
        /// 七乐彩
        /// </summary>
        Qlc = 4,

        /// <summary>
        /// 七星彩
        /// </summary>
        Qxc = 5,

        #region 十一选五
        /// <summary>
        /// 黑龙江十一选五
        /// </summary>
        HlSyxw = 10101,

        /// <summary>
        /// 吉林十一选五
        /// </summary>
        JlSyxw = 10102,

        /// <summary>
        /// 辽宁十一选五
        /// </summary>
        LnSyxw = 10103,

        /// <summary>
        /// 广东十一选五
        /// </summary>
        GdSyxw = 10104,

        /// <summary>
        /// 山东十一选五
        /// </summary>
        SdSyxw = 10105,

        /// <summary>
        /// 江西十一选五
        /// </summary>
        JxSyxw = 10106,

        /// <summary>
        /// 重庆十一选五
        /// </summary>
        CqSyxw = 10107,

        /// <summary>
        /// 河北十一选五
        /// </summary>
        HeSyxw = 10108,

        /// <summary>
        /// 天津十一选五
        /// </summary>
        TjSyxw = 10109,

        /// <summary>
        /// 北京十一选五
        /// </summary>
        BjSyxw = 10110,

        /// <summary>
        /// 四川十一选五
        /// </summary>
        ScSyxw = 10111,

        /// <summary>
        /// 湖北十一选五
        /// </summary>
        HbSyxw = 10118,

        /// <summary>
        /// 陕西十一选五
        /// </summary>
        SnSyxw = 10112,

        /// <summary>
        /// 河南十一选五
        /// </summary>
        HaSyxw = 10113,

        /// <summary>
        /// 甘肃十一选五
        /// </summary>
        GsSyxw = 10124,

        /// <summary>
        /// 江苏十一选五
        /// </summary>
        JsSyxw = 10115,

        /// <summary>
        /// 浙江十一选五
        /// </summary>
        ZjSyxw = 10123,

        /// <summary>
        /// 内蒙十一选五
        /// </summary>
        NmSyxw = 10121,

        /// <summary>
        /// 广西十一选五
        /// </summary>
        GxSyxw = 10122,

        /// <summary>
        /// 上海十一选五
        /// </summary>
        ShSyxw = 10116,

        /// <summary>
        /// 安徽十一选五
        /// </summary>
        AhSyxw = 10117,

        /// <summary>
        /// 青海十一选五
        /// </summary>
        QhSyxw = 10114,

        /// <summary>
        /// 新疆十一选五
        /// </summary>
        XjSyxw = 10119,

        /// <summary>
        /// 云南十一选五
        /// </summary>
        YnSyxw = 10120,

        /// <summary>
        /// 福建十一选五
        /// </summary>
        FjSyxw = 10125,

        /// <summary>
        /// 贵州十一选五
        /// </summary>
        GzSyxw = 10126,

        /// <summary>
        /// 山西十一选五
        /// </summary>
        SxSyxw = 10127,

        /// <summary>
        /// 西藏十一选五
        /// </summary>
        XzSyxw = 10128,

        /// <summary>
        /// 宁夏十一选五
        /// </summary>
        NxSyxw = 10129,

        #endregion

        #region 快乐十分

        /// <summary>
        /// 龙江快乐十分
        /// </summary>
        HlKlsf = 10301,

        /// <summary>
        /// 广东快乐十分
        /// </summary>
        GdKlsf = 10302,

        /// <summary>
        /// 重庆快乐十分
        /// </summary>
        CqKlsf = 10303,

        /// <summary>
        /// 天津快乐十分
        /// </summary>
        TjKlsf = 10304,

        /// <summary>
        /// 陕西快乐十分
        /// </summary>
        SnKlsf = 10305,

        /// <summary>
        /// 云南快乐十分
        /// </summary>
        YnKlsf = 10306,

        /// <summary>
        /// 广西快乐十分
        /// </summary>
        GxKlsf = 10307,

        /// <summary>
        /// 山西快乐十分
        /// </summary>
        SxKlsf = 10308,

        #endregion

        #region 快三
        /// <summary>
        /// 吉林快三
        /// </summary>
        JlKs = 10401,

        /// <summary>
        /// 江苏快三
        /// </summary>
        JsKs = 10402,

        /// <summary>
        /// 河北快三
        /// </summary>
        HeKs = 10403,

        /// <summary>
        /// 内蒙快三
        /// </summary>
        NmKs = 10404,

        /// <summary>
        /// 安徽快三
        /// </summary>
        AhKs = 10405,

        /// <summary>
        /// 北京快三
        /// </summary>
        BjKs = 10406,

        /// <summary>
        /// 河南快三
        /// </summary>
        HaKs = 10407,

        /// <summary>
        /// 宁夏快三
        /// </summary>
        NxKs = 10408,

        /// <summary>
        /// 上海快三
        /// </summary>
        ShKs = 10409,

        /// <summary>
        /// 湖北快三
        /// </summary>
        HbKs = 10410,

        /// <summary>
        /// 广西快三
        /// </summary>
        GxKs = 10411,

        /// <summary>
        /// 江西快三
        /// </summary>
        JxKs = 10412,

        /// <summary>
        /// 甘肃快三
        /// </summary>
        GsKs = 10413,

        /// <summary>
        /// 青海快三
        /// </summary>
        QhKs = 10414,

        /// <summary>
        /// 西藏快三
        /// </summary>
        XzKs = 10415,

        #endregion

        /// <summary>
        /// 重庆百变王牌
        /// </summary>
        CqBbwp = 10501,

        /// <summary>
        /// 重庆新百变王牌十四选五开六，10分钟
        /// </summary>
        CqBbwp_140610 = 10502,

        /// <summary>
        /// 重庆新百变王牌十七选五开七，10分钟
        /// </summary>
        CqBbwp_170710 = 10503,

        /// <summary>
        /// 重庆新百变王牌十四选五开六，10分钟
        /// </summary>
        CqBbwp_140605 = 10504,

        /// <summary>
        /// 天津时时彩
        /// </summary>
        TjSsc = 10601,

        /// <summary>
        /// 重庆时时彩
        /// </summary>
        CqSsc = 10604,

        /// <summary>
        /// 河南泳坛夺金
        /// </summary>
        HaYtdj = 10701,

        /// <summary>
        /// 河南幸运彩
        /// </summary>
        HaXyc = 10702,

        /// <summary>
        /// 江西E球彩
        /// </summary>
        JxEqc = 10703,

        /// <summary>
        /// 甘肃泳坛夺金
        /// </summary>
        GsYtdj = 10704,

        /// <summary>
        /// 山东群英会
        /// </summary>
        SdQyh = 10801,

        /// <summary>
        /// 湖南幸运赛车
        /// </summary>
        HnXysc = 10802,

        /// <summary>
        /// 四川金七乐
        /// </summary>
        ScJql = 10901,

        /// <summary>
        /// 竞彩足球
        /// </summary>
        JcZc = 20200,
        
        /// <summary>
        /// 竞彩胜平负
        /// </summary>
        JcSpf = 20201,

        /// <summary>
        /// 竞彩比分
        /// </summary>
        JcBf = 20202,

        /// <summary>
        /// 竞彩总进球
        /// </summary>
        JcZjq = 20203,

        /// <summary>
        /// 竞彩半全场
        /// </summary>
        JcBqc = 20204,

        /// <summary>
        /// 竞彩混合投注
        /// </summary>
        JcHun = 20205,

        /// <summary>
        /// 竞彩让球胜平负
        /// </summary>
        JcRqspf = 20206,

        /// <summary>
        /// 足彩十四场
        /// </summary>
        ZcSfc = 20101,

        /// <summary>
        /// 足彩任九
        /// </summary>
        ZcR9 = 20102,

        /// <summary>
        /// 足彩6场半全场
        /// </summary>
        ZcBqc = 20103,

        /// <summary>
        /// 足彩4场进球彩
        /// </summary>
        ZcJqc = 20104,

        /// <summary>
        /// 竞彩篮球
        /// </summary>
        JcLc = 20400,

        /// <summary>
        /// 竞彩篮球胜负
        /// </summary>
        LcSf = 20401,

        /// <summary>
        /// 竞彩篮球让分胜负
        /// </summary>
        LcRfsf = 20402,

        /// <summary>
        /// 竞彩篮球胜分差
        /// </summary>
        LcSfc = 20403,

        /// <summary>
        /// 竞彩篮球大小分
        /// </summary>
        LcDxf = 20404,

        /// <summary>
        /// 竞彩篮球混合投注
        /// </summary>
        LcHun = 20405,

    }
}
