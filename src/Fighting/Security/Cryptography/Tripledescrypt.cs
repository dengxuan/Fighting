using Fighting.Extensions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fighting.Security.Cryptography
{
    /// <summary>
    /// 3DES加密
    /// </summary>
    public sealed class Tripledescrypt : IEncryptable, IDecryptable
    {
        /// <summary>
        /// IV_64 向量
        /// </summary>
        private string _IV_64 = "12345678";

        /// <summary>
        /// 加密模式
        /// </summary>
        private CipherMode _chipher { get; set; }

        /// <summary>
        /// 补位模式
        /// </summary>
        private PaddingMode _padding { get; set; }

        private Tripledescrypt() { }

        /// <summary>
        /// 构造3DES加密对象，使用CBC加密模式，无补位
        /// </summary>
        public static Tripledescrypt Create()
        {
            Tripledescrypt instance = new Tripledescrypt();
            instance.Init(CipherMode.CBC, PaddingMode.None);
            return instance;
        }

        /// <summary>
        /// 构造3DES加密对象，无补位
        /// </summary>
        /// <param name="cipher">加密模式</param>
        public static Tripledescrypt Create(CipherMode cipher)
        {
            Tripledescrypt instance = new Tripledescrypt();
            instance.Init(cipher, PaddingMode.None);
            return instance;
        }

        /// <summary>
        /// 构造3DES加密对象
        /// </summary>
        /// <param name="cipher">加密模式</param>
        /// <param name="padding">补位方式</param>
        public static Tripledescrypt Create(CipherMode cipher, PaddingMode padding)
        {
            Tripledescrypt instance = new Tripledescrypt();
            instance.Init(cipher, padding);
            return instance;
        }

        /// <summary>
        /// 初始化加密对象
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="cipher">加密模式</param>
        /// <param name="padding">补位模式</param>
        public void Init(CipherMode cipher, PaddingMode padding)
        {
            this._chipher = cipher;
            this._padding = padding;
        }

        /// <summary>
        /// 加密,加密流使用UTF8编码
        /// </summary>
        /// <param name="input">待加密字符串,明文</param>
        /// <returns>加密后的密文</returns>
        public string Encrypt(string input, string key)
        {
            return this.Encrypt(input, key, Encoding.UTF8);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input">待加密字符串,明文</param>
        /// <param name="encoding">加密流使用的编码方式</param>
        /// <returns>以base64编码后的加密字符串,密文</returns>
        public string Encrypt(string input, string key, Encoding encoding)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            byte[] iv = Encoding.ASCII.GetBytes(_IV_64);

            //设置加密方式
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider
            {
                Mode = _chipher,
                Padding = _padding
            };

            //加密
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, tdsp.CreateEncryptor(keyBytes, iv), CryptoStreamMode.Write);
            byte[] bytes = encoding.GetBytes(input);

            //将加密的数据流写入内存流
            cStream.Write(bytes, 0, bytes.Length);
            cStream.FlushFinalBlock();

            //从加密后的内存流中获取字节数组
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            // 将加密数据转换为Base64字符串返回
            return Convert.ToBase64String(ret);
        }

        /// <summary>
        /// 解密,解密流编码方式使用UTF8
        /// </summary>
        /// <param name="input">待解密字符串,密文</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后的字符串,明文</returns>
        public string Decrypt(string input, string key)
        {
            return this.Decrypt(input, key, Encoding.UTF8);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input">待解密字符串</param>
        /// <param name="encoding">解密流使用的编码方式,密文</param>
        /// <returns>解密后的字符串,明文</returns>
        public String Decrypt(string input, string key, Encoding encoding)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            byte[] iv = Encoding.ASCII.GetBytes(_IV_64);

            //根据密文获取base64编码字节数组
            byte[] bytes = Convert.FromBase64String(input);

            //设置解密方式
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider
            {
                Mode = _chipher,
                Padding = _padding
            };

            //解密
            MemoryStream mStream = new MemoryStream(bytes);
            CryptoStream cStream = new CryptoStream(mStream, tdsp.CreateDecryptor(keyBytes, iv), CryptoStreamMode.Read);
            byte[] fromEncrypt = new byte[input.Length];
            //将密文流读入内存流,用指定格式编码为字符串返回
            cStream.Read(fromEncrypt, 0, fromEncrypt.Length);
            return encoding.GetString(fromEncrypt);
        }
    }
}
