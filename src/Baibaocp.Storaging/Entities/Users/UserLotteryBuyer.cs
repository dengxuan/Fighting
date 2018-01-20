using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Users
{
    /// <summary>
    /// 百宝彩彩民
    /// </summary>
    [Table("BbcpUserLotteryBuyers")]
    public class UserLotteryBuyer : Entity<long>
    {
        /// <summary>
        /// 昵称 <see cref="Nickname"/> 最大长度
        /// </summary>
        public const int MaxNicknameLength = 10;

        /// <summary>
        /// 昵称 <see cref="Nickname"/> 正则表达式
        /// </summary>
        public const string NicknameRegex = @"^([\u4e00-\u9fa5]+|([a-z]+\s?)+)$";

        /// <summary>
        /// 真实姓名 <see cref="Realname"/> 最大长度
        /// </summary>
        public const int MaxRealnameLength = 10;

        /// <summary>
        /// 真实姓名 <see cref="Realname"/> 正则表达式
        /// </summary>
        public const string RealnameRegex = @"^([\u4e00-\u9fa5]+|([a-z]+\s?)+)$";

        /// <summary>
        /// 密码 <see cref="Password"/> 最大长度
        /// </summary>
        public const int MaxPasswordLength = 128;

        /// <summary>
        /// 证件编号<see cref="CredentialsNumber"/> 最大长度
        /// </summary>
        public const int MaxCredentialsNumberLength = 18;

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(MaxNicknameLength)]
        [RegularExpression(NicknameRegex)]
        public string Nickname { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(MaxRealnameLength)]
        [RegularExpression(RealnameRegex)]
        public string Realname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(MaxPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 彩民手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public short CredentialsTypeId { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        [ForeignKey("CredentialsTypeId")]
        public virtual CredentialsType CredentialsType { get; set; }

        /// <summary>
        /// 证件编号
        /// </summary>
        [StringLength(MaxCredentialsNumberLength)]
        //[RegularExpression(IDCardRegex)]
        public string CredentialsNumber { get; set; }

        /// <summary>
        /// 彩民Email
        /// </summary>
        public string Email { get; set; }
            
        /// <summary>
        /// 彩民所在省份
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 彩民所在省份 <see cref="Regions.Province"/>
        /// </summary>
        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }

        /// <summary>
        /// 彩民所属市
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// 彩民所属市 <see cref="Regions.City"/>
        /// </summary>
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        /// <summary>
        /// 彩民所属区县
        /// </summary>
        public int? TownId { get; set; }

        /// <summary>
        /// 彩民所属区县 <see cref="Regions.Town"/>
        /// </summary>
        [ForeignKey("TownId")]
        public virtual Town Town { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

    }
}
