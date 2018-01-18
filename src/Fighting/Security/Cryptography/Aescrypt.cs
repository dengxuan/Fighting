using System;
using System.Text;

namespace Fighting.Security.Cryptography
{
    /// <summary>
    /// AES加密解密
    /// </summary>
    public sealed class Aescrypt : IEncryptable, IDecryptable
    {
        /// <summary>
        /// IV_64 向量
        /// </summary>
        private String _IV_64 = "12345678";


        public string Encrypt(string input, string key)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string input, string key, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string input, string key)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string input, string key, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}
