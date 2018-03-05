using System.Text;

namespace Fighting.Security.Cryptography
{
    /// <summary>
    /// 加密接口
    /// </summary>
    public interface IDecryptable
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        string Decrypt(string input, string key);

        /// <summary>
        /// 用指定编码解密
        /// </summary>
        /// <param name="input">密文</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        string Decrypt(string input, string key, Encoding encoding);
    }
}
