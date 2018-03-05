using System.Text;

namespace Fighting.Security.Cryptography
{
    /// <summary>
    /// 加密接口
    /// </summary>
    public interface IEncryptable
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        string Encrypt(string input, string key);

        /// <summary>
        /// 用指定编码加密
        /// </summary>
        /// <param name="input">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>密文</returns>
        string Encrypt(string input, string key, Encoding encoding);
    }
}
